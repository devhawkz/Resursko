using Microsoft.AspNetCore.Identity;

namespace Resursko.API.Respositories.UsersRespository;

public class UsersRespository(DataContext context, UserManager<User> userManager) : IUsersRespository
{
    public async Task<List<GetAllUsersResponse>> GetAllUsers()
    {
        var userResponses = new List<GetAllUsersResponse>();

        var users = await context.Users
            .Include(u => u.Reservations)
            .ToListAsync();

        foreach (var user in users)
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
}
