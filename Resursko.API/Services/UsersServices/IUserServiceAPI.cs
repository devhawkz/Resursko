namespace Resursko.API.Services.UsersServices;

public interface IUserServiceAPI
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
    Task<AccountLoginResponse> UpdateUserInfo(UpdateUsersInfoRequest request);
    Task<AccountResponse> DeleteAccount();
    Task<GetAllUsersResponse> GetUserInfo();
}
