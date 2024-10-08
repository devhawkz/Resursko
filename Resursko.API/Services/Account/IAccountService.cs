﻿using Resursko.Domain.DTOs.Account;

namespace Resursko.API.Services.Account;

public interface IAccountService
{
    Task<AccountRegistrationResponse> RegisterAsync(AccountRegistrationRequest request);
    Task<AccountLoginResponse> LoginAsync(AccountLoginRequest request);
    Task<TokenRefreshRequest> RefreshToken(TokenRefreshRequest request);
}
