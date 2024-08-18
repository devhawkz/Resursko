using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Resursko.Client.AuthProviders;
using Resursko.Domain.DTOs.Account;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Resursko.Client.Services.UsersService;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    public UserService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }
    public async Task<List<GetAllUsersResponse>> GetAllUsers()
    {
        var response = await _httpClient.GetAsync("api/administrator");
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) 
            return new List<GetAllUsersResponse>();

        return JsonSerializer.Deserialize<List<GetAllUsersResponse>>(responseContent, _jsonSerializerOptions)!;
    }

    public async Task<AccountLoginResponse> UpdateUserInfo(UpdateUsersInfoRequest request)
    {
        var content = JsonSerializer.Serialize(request);
        var contentBody = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync("api/users/update-info", contentBody);
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<AccountLoginResponse>(responseContent, _jsonSerializerOptions);

        if (!response.IsSuccessStatusCode)
            return result!;

        await _localStorage.SetItemAsync("authToken", result!.Token);
        await _localStorage.SetItemAsync("refreshToken", result!.RefreshToken);

        ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result!.Token);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

        return new AccountLoginResponse(true);
    }

    public async Task<AccountResponse> DeleteAccount()
    {
        var response = await _httpClient.DeleteAsync("api/users/delete-account");
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<AccountResponse>(responseContent, _jsonSerializerOptions)!;

        return new AccountResponse(true);
    }

    public async Task<GetAllUsersResponse> GetUserInfo()
    {
        var response = await _httpClient.GetAsync("api/users/user-info");
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return new GetAllUsersResponse();

        return JsonSerializer.Deserialize<GetAllUsersResponse>(responseContent, _jsonSerializerOptions)!;
    }
}
