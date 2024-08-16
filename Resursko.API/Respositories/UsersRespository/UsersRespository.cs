using Microsoft.AspNetCore.Identity;
using Resursko.API.Services.EmailService;
using Resursko.API.Services.UserContext;
using System;
using System.Runtime.InteropServices;

namespace Resursko.API.Respositories.UsersRespository;

public class UsersRespository(DataContext context, UserManager<User> userManager, IEmailSenderAsync emailSender) : IUsersRespository
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
                Username = user.UserName,
                ActiveReservations = activeReservations,
                Roles = roles.ToList()
            };

            userResponses.Add(userResponse);
        }

        return userResponses;
    }
    public async Task<AccountResponse> UpdateUserInfo(User user)
    {
        var dbUser = await context.Users.FindAsync(user.Id);
        if (dbUser is null)
            return new AccountResponse(false, $"There is no user with Id: {user.Id} in database");

        dbUser.FirstName = user.FirstName;
        dbUser.LastName = user.LastName;
        dbUser.Email = user.Email;
        dbUser.UserName = user.UserName;

        await context.SaveChangesAsync();

        var message = new Message(new string[] { user.Email! }, "Update profile info", "You have successfully updated your personal information!");
        await emailSender.SendEmailAsync(message);

        _isListChanged = true;
        return new AccountResponse(true);
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
}
