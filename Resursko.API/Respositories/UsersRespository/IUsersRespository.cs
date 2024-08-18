namespace Resursko.API.Respositories.UsersRespository;

public interface IUsersRespository
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
    Task<AccountLoginResponse> UpdateUserInfo(User user);
    Task<AccountResponse> DeleteAccount(string id);
    Task<GetAllUsersResponse> GetUserInfo(string? id);

}
