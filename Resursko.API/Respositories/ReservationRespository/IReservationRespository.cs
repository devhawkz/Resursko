using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Respositories.ReservationRespository;

public interface IReservationRespository
{
    Task<List<GetAllReservationResponse>> GetAllReservations();
}
