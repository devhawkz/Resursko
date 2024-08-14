using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Resursko.Domain.DTOs.Account;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Resursko.Client.AuthProviders;

namespace Resursko.Client.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        public AccountService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
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
                return result!;
            }
            return new AccountRegistrationResponse(true);
        }

        public async Task<AccountLoginResponse> Login(AccountLoginRequest request)
        {
            var content = JsonSerializer.Serialize(request);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var loginResult = await _httpClient.PostAsync("api/account", bodyContent);
            var loginContent = await loginResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AccountLoginResponse>(loginContent, _jsonSerializerOptions);

            if (!loginResult.IsSuccessStatusCode)
                return result!;

            await _localStorage.SetItemAsync("authToken", result!.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result!.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return new AccountLoginResponse(true);
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

    }
}
