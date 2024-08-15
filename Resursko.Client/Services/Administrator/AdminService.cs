using Microsoft.AspNetCore.Components.Authorization;
using Resursko.Domain.DTOs.Account;
using System.Text;
using System.Text.Json;

namespace Resursko.Client.Services.Administrator;

public class AdminService : IAdminService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly AuthenticationStateProvider _authStateProvider;
    public AdminService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _authStateProvider = authStateProvider;
    }
    public async Task<AccountRegistrationResponse> RegisterAdmin(AccountRegistrationRequest request)
    {
        var content = JsonSerializer.Serialize(request);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/administrator", bodyContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if(!response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<AccountRegistrationResponse>(responseContent, _jsonSerializerOptions)!;

        return new AccountRegistrationResponse(true);
    }
}
