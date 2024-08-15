using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Resursko.Domain.DTOs.ReservationDTO;
using System.Text;
using System.Text.Json;

namespace Resursko.Client.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        public ReservationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }
        public async Task<ReservationResponse> CreateNewReservation(ReservationRequest request)
        {
            var content = JsonSerializer.Serialize(request);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/reservation", bodyContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<ReservationResponse>(responseContent, _jsonSerializerOptions)!;

            return new ReservationResponse(true);
        }

        public async Task<List<GetAllReservationResponse>> GetAllReservations()
        {
            var response = await _httpClient.GetAsync("api/reservation");
            var responseContent = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
                return new List<GetAllReservationResponse>();

            return JsonSerializer.Deserialize<List<GetAllReservationResponse>>(responseContent, _jsonSerializerOptions)!;
        }

        public async Task<ReservationResponse> UpdateReservation(ReservationRequest request, int id)
        {
            var content = JsonSerializer.Serialize(request);
            var contentBody = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/reservation/{id}", contentBody);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<ReservationResponse>(responseContent, _jsonSerializerOptions)!;

            return new ReservationResponse(true);

        }
        public async Task<ReservationResponse> DeleteReservation(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/reservation/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<ReservationResponse>(responseContent, _jsonSerializerOptions)!;

            return new ReservationResponse(true);
        }
    }
}
