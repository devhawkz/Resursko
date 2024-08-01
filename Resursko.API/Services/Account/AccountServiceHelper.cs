using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Resursko.API.Services.Account;

public class AccountServiceHelper(RoleManager<Role> roleManager)
{
    private readonly string _adminRole = "Admin";
    private readonly string _userRole = "User";
    public async Task<Role> GetRoleAsync(string flag)
    {
        var roleName = _userRole;
        if (flag.Equals("admin"))
            roleName = _adminRole;

        var roleExists = await roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            var role = await CreateNewRole(roleName);
            return role;
        }

        else
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role is null)
                throw new InvalidOperationException($"Failed to find role: {roleName}");

            return role;
        }

    }

    private async Task<Role> CreateNewRole(string roleName)
    {
        var role = new Role
        {
            Name = roleName,
            NormalizedName = roleName.ToUpper(),
            Description = $"{roleName} role for user"
        };

        var roleCreated = await roleManager.CreateAsync(role);

        if (!roleCreated.Succeeded)
            throw new InvalidOperationException($"Failed to create role: {roleName}");

        return role;
    }

    public User GetUser(AccountRegistrationRequest request)
    {
        var newUser = request.Adapt<User>();
        newUser.UserName = request.Username;

        return newUser;
    }
}
