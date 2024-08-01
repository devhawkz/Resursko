using Resursko.Domain.DTOs.Account;

namespace Resursko.API.Services.Account;

public interface IAccountService
{
    Task<AccountRegistrationResponse> RegisterAsync(AccountRegistrationRequest request);
}
