using Resursko.Domain.DTOs.Account;

namespace Resursko.Client.Services.Administrator;

public interface IAdminService
{
    Task<AccountRegistrationResponse> RegisterAdmin(AccountRegistrationRequest request); 
}
