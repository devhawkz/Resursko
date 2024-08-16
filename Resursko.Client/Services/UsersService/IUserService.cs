using Resursko.Domain.DTOs.Account;

namespace Resursko.Client.Services.UsersService;

public interface IUserService
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
}
