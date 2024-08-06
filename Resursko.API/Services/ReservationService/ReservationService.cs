using Mapster;
using Resursko.API.Respositories.ReservationRespository;
using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Services.ReservationService;

public class ReservationService(IReservationRespository reservationRespository) : IReservationService
{
    public async Task<ReservationResponse> CreateNewReservation(ReservationRequest request)
    {
        var newReservation = request.Adapt<Reservation>();
        var result = await reservationRespository.CreateNewReservation(newReservation);
        return result;
    }

    public async Task<List<GetAllReservationResponse>> GetAllReservations()
    {
        var result = await reservationRespository.GetAllReservations();

        if (result is null || result.Count == 0)
            return new List<GetAllReservationResponse>();

        return result;
    }

    public async Task<ReservationResponse> UpdateReservation(ReservationRequest request, int id)
    {
        if (id <= 0)
            return new ReservationResponse(false, $"Reservation id can't have this value: {id}");

        var updatedReservation = request.Adapt<Reservation>();
        var result = await reservationRespository.UpdateReservation(updatedReservation, id);

        return result;
    }

    public async Task<ReservationResponse> DeleteReservation(int id)
    {
        if (id <= 0)
            return new ReservationResponse(false, $"Reservation id can't have this value: {id}");

        var result = await reservationRespository.DeleteReservation(id);
        return result;
    }
}
