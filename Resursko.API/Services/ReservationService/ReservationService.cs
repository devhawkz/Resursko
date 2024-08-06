using Resursko.API.Respositories.ReservationRespository;
using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Services.ReservationService;

public class ReservationService(IReservationRespository reservationRespository) : IReservationService
{
    public async Task<List<GetAllReservationResponse>> GetAllReservations()
    {
        var result = await reservationRespository.GetAllReservations();

        if (result is null || result.Count == 0)
            return new List<GetAllReservationResponse>();

        return result;
    }
}
