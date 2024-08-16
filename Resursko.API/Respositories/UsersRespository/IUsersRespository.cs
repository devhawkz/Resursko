namespace Resursko.API.Respositories.UsersRespository;

public interface IUsersRespository
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
}
