using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Services.ReservationService;

public interface IReservationService
{
    Task<List<GetAllReservationResponse>> GetAllReservations();
}
