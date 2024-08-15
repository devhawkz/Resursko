using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.Client.Services.ReservationServices;

public interface IReservationService
{
    Task<ReservationResponse> CreateNewReservation(ReservationRequest request);
    Task<List<GetAllReservationResponse>> GetAllReservations();
    Task<ReservationResponse> DeleteReservation(int id);
}
