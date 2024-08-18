using Resursko.Domain.DTOs.Account;

namespace Resursko.Client.Services.UsersService;

public interface IUserService
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
    Task<AccountLoginResponse> UpdateUserInfo(UpdateUsersInfoRequest request);
    Task<AccountResponse> DeleteAccount();
    Task<GetAllUsersResponse> GetUserInfo();
}
