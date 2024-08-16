﻿using Resursko.Domain.DTOs.Account;

namespace Resursko.Client.Services.UsersService;

public interface IUserService
{
    Task<List<GetAllUsersResponse>> GetAllUsers();
    Task<AccountResponse> UpdateUserInfo(UpdateUsersInfoRequest request);
    Task<AccountResponse> DeleteAccount();
}
