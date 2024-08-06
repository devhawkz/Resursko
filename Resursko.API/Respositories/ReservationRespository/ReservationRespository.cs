

using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Respositories.ReservationRespository;

public class ReservationRespository(DataContext context) : IReservationRespository
{
    public async Task<List<GetAllReservationResponse>> GetAllReservations()
    {
        return await context.Reservations
            .Include(r => r.User)
            .Include(r => r.Resource)
            .Select(r => new GetAllReservationResponse
            {
                Username = r.User.UserName,
                Email = r.User.Email,
                ResourceName = r.Resource.Name,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status
            })
            .ToListAsync();
    }
}
