﻿using Resursko.Domain.DTOs.Account;

namespace Resursko.Client.Services.Account;

public interface IAccountService
{
    Task<AccountRegistrationResponse> Register(AccountRegistrationRequest request);
    Task<AccountLoginResponse> Login(AccountLoginRequest request);
    Task Logout();

    Task<string> RefreshToken();
}
