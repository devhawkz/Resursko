
using Resursko.API.Respositories.UsersRespository;

namespace Resursko.API.Services.UsersServices;

public class UserService(IUsersRespository usersRespository) : IUserService
{
    public async Task<List<GetAllUsersResponse>> GetAllUsers()
    {
        var result = await usersRespository.GetAllUsers();

        if (result is null || result.Count == 0)
            return new List<GetAllUsersResponse>();

        return result;
    }
}
