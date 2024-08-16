namespace Resursko.API.Services.UsersServices;

public interface IUserService
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
}
