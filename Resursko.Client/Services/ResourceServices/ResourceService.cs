using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Resursko.Domain.DTOs.ResourceDTO;
using System.Text;
using System.Text.Json;

namespace Resursko.Client.Services.ResourceServices;

public class ResourceService : IResourceService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    public ResourceService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<ResourceResponse> CreateResource(ResourceRequest request)
    {
        var content = JsonSerializer.Serialize(request);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/resource", bodyContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<ResourceResponse>(responseContent, _jsonSerializerOptions)!;

        return new ResourceResponse(true);
    }

    public async Task<List<GetResourcesDTO>> GetAllResources()
    {
        var response = await _httpClient.GetAsync("api/resource");
        var content = await response.Content.ReadAsStringAsync();
        
        if(!response.IsSuccessStatusCode)
            return new List<GetResourcesDTO>();

        return JsonSerializer.Deserialize<List<GetResourcesDTO>>(content, _jsonSerializerOptions)!;
    }

    public async Task<ResourceResponse> DeleteResource(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/resources/{id}");
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<ResourceResponse>(responseContent, _jsonSerializerOptions)!;

        return new ResourceResponse(true);
    }
}
