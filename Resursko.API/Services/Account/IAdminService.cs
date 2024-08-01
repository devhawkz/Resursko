namespace Resursko.API.Services.Account;
public interface IAdminService
{
    Task<AccountRegistrationResponse> RegisterAdminAsync(AccountRegistrationRequest request);
}
