namespace Resursko.API.Services.UsersServices;

public interface IUserServiceAPI
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
    Task<AccountResponse> UpdateUserInfo(UpdateUsersInfoRequest request);
    Task<AccountResponse> DeleteAccount();
}
