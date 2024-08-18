
using Mapster;
using Resursko.API.Respositories.UsersRespository;
using Resursko.API.Services.UserContext;
using Resursko.Domain.Models;

namespace Resursko.API.Services.UsersServices;

public class UserServiceAPI(IUsersRespository usersRespository, IUserContextService userContextService) : IUserServiceAPI
{
    public async Task<List<GetAllUsersResponse>> GetAllUsers()
    {
        var result = await usersRespository.GetAllUsers();

        if (result is null || result.Count == 0)
            return new List<GetAllUsersResponse>();

        return result;
    }

    public async Task<AccountLoginResponse> UpdateUserInfo(UpdateUsersInfoRequest request)
    {
        var userId = userContextService.GetUserId();
        if (userId is null)
            return new AccountLoginResponse(false, ErrorMessage: "You must login to perform this action!");

        var user = request.Adapt<User>();
        user.Id = userId;

        var result = await usersRespository.UpdateUserInfo(user);
        return result;


    }

    public async Task<AccountResponse> DeleteAccount()
    {
        var userId = userContextService.GetUserId();
        if (userId is null)
            return new AccountResponse(false, "You must login to perform this action!");

        var result = await usersRespository.DeleteAccount(userId);
        return result;
    }

    public async Task<GetAllUsersResponse> GetUserInfo()
    {
        var id = userContextService.GetUserId();
        var result = await usersRespository.GetUserInfo(id);
        if(result is null)
            return new GetAllUsersResponse();

        return result!;
    }
}
