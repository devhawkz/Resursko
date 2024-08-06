using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Respositories.ReservationRespository;

public interface IReservationRespository
{
    Task<ReservationResponse> CreateNewReservation(Reservation reservation);
    Task<List<GetAllReservationResponse>> GetAllReservations();
}
