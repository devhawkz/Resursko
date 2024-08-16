namespace Resursko.API.Respositories.UsersRespository;

public interface IUsersRespository
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
    Task<AccountResponse> UpdateUserInfo(User user);
    Task<AccountResponse> DeleteAccount(string id);
}
