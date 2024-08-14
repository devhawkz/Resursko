using Resursko.Domain.DTOs.Account;
using System.Text;
using System.Text.Json;

namespace Resursko.Client.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
        }
        public async Task<AccountRegistrationResponse> Register(AccountRegistrationRequest request)
        {
            var content = JsonSerializer.Serialize(request);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var registrationResult = await _httpClient.PostAsync("api/account/registration", bodyContent);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();
            
            if(!registrationResult.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<AccountRegistrationResponse>(registrationContent, _jsonSerializerOptions);
                return result;
            }
            return new AccountRegistrationResponse(true);
        }
    }
}
