using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Services.ReservationService;

public interface IReservationService
{
    Task<ReservationResponse> CreateNewReservation(ReservationRequest request);
    Task<List<GetAllReservationResponse>> GetAllReservations();
    Task<ReservationResponse> UpdateReservation(ReservationRequest request, int id);
}
