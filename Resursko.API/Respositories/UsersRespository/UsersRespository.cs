using Mapster;
using Microsoft.AspNetCore.Identity;
using Resursko.API.Services.EmailService;
using Resursko.API.Services.JwtHandler;
using Resursko.API.Services.UserContext;
using System;
using System.Runtime.InteropServices;

namespace Resursko.API.Respositories.UsersRespository;

public class UsersRespository(DataContext context, UserManager<User> userManager, IEmailSenderAsync emailSender, JwtService jwtService) : IUsersRespository
{

    private static List<User> _users = new List<User>();
    private static bool _isListChanged = false;
    public async Task<List<GetAllUsersResponse>> GetAllUsers()
    {
       var userResponses = new List<GetAllUsersResponse>();

        if (_isListChanged || _users.Count == 0)
            await GetUsers();

        foreach (var user in _users)
        {
            var roles = await userManager.GetRolesAsync(user);
            var activeReservations = user.Reservations.Count(r => r.Status == "active");

            var userResponse = new GetAllUsersResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                ActiveReservations = activeReservations,
                Roles = roles.ToList()
            };

            userResponses.Add(userResponse);
        }

        return userResponses;
    }
    public async Task<AccountLoginResponse> UpdateUserInfo(User user)
    {
        var dbUser = await context.Users.FindAsync(user.Id);
        if (dbUser is null)
            return new AccountLoginResponse(false, ErrorMessage: $"There is no user with Id: {user.Id} in database");

        if(!dbUser.Email!.Equals(user.Email))
        {
            var token = await userManager.GenerateChangeEmailTokenAsync(dbUser, user.Email!);
            var result = await userManager.ChangeEmailAsync(dbUser, user.Email!, token);

            if (!result.Succeeded)
                return new AccountLoginResponse(false, ErrorMessage: "Unsuccessful email address change!");
        }

        

        dbUser.FirstName = user.FirstName;
        dbUser.LastName = user.LastName;
        dbUser.UserName = user.UserName;

        await context.SaveChangesAsync();

        var message = new Message(new string[] { user.Email! }, "Update profile info", "You have successfully updated your personal information!");
        await emailSender.SendEmailAsync(message);

        _isListChanged = true;

        var roles = await userManager.GetRolesAsync(dbUser);
        var jwtToken = await jwtService.CreateToken(dbUser, roles, true);

        return new AccountLoginResponse(true, Token: jwtToken, RefreshToken: dbUser.RefreshToken!);
    }
    public async Task<AccountResponse> DeleteAccount(string id)
    {
        var userToDelete = await context.Users.FindAsync(id);
        if (userToDelete is null)
            return new AccountResponse(false, $"User with id: {id} doesn't exist!");

        context.Users.Remove(userToDelete);
        await context.SaveChangesAsync();

        var message = new Message(new string[] { userToDelete.Email! }, "Removing account", "You have deleted your account!");
        await emailSender.SendEmailAsync(message);
        _isListChanged = true;
        return new AccountResponse(true);
    }
    private async Task<List<User>> GetUsers()
    {
        _users = await context.Users
            .Include(u => u.Reservations)
            .ToListAsync();

        return _users;
    }

    public async Task<GetAllUsersResponse> GetUserInfo(string id)
    {
            await GetUsers();

        var user = _users!.Find(u => u.Id == id);
        
        var activeReservations = user!.Reservations.Count(r => r.Status == "active");
        var roles = await userManager.GetRolesAsync(user);
        
        var userResponse = new GetAllUsersResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName,
            ActiveReservations = activeReservations,
            Roles = roles.ToList()
        };
        return userResponse;
    }
}
