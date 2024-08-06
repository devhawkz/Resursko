using Resursko.API.Services.UserContext;
using Resursko.Domain.DTOs.ReservationDTO;
using Resursko.Domain.Models;

namespace Resursko.API.Respositories.ReservationRespository;

public class ReservationRespository(DataContext context, IUserContextService userContextService) : IReservationRespository
{
    public async Task<ReservationResponse> CreateNewReservation(Reservation reservation)
    {
        var userId = userContextService.GetUserId();
        if (userId is null)
            return new ReservationResponse(false, "You must log in to perform this action!");

        if (reservation.StartTime > reservation.EndTime)
            return new ReservationResponse(false, "Start time can't be older than end time");

        reservation.Resource = await context.Resources.FindAsync(reservation.ResourceId);
        reservation.User = await context.Users.FindAsync(userId);

        if (!IsResourceAvailable(reservation.Resource!))
            return new ReservationResponse(false, "Resource is not available in selected period.");

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();
        
        return new ReservationResponse(true);
    }

    public async Task<List<GetAllReservationResponse>> GetAllReservations()
    {
        await SetStatus();

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

    public async Task<ReservationResponse> UpdateReservation(Reservation reservation, int id)
    {
        var userId = userContextService.GetUserId();
        if (userId is null)
            return new ReservationResponse(false, "You must log in to perform this action!");

        if(reservation.StartTime > reservation.EndTime)
            return new ReservationResponse(false, "Start time can't be older than end time");

        var dbReservation = await context.Reservations.FindAsync(id);
        if (dbReservation is null)
            return new ReservationResponse(false, $"Reservation with id: {id} doesn't exist");

        var resource = await context.Resources.FindAsync(reservation.ResourceId);
        if (resource is null)
            return new ReservationResponse(false, $"Resource with id: {reservation.ResourceId} doesn't exist");

        dbReservation.StartTime = reservation.StartTime;
        dbReservation.EndTime = reservation.EndTime;
        dbReservation.Status = reservation.Status;
        dbReservation.Resource =  resource!;

        await context.SaveChangesAsync();

        return new ReservationResponse(true);
    }

    public async Task<ReservationResponse> DeleteReservation(int id)
    {
        var reservation = await context.Reservations.FindAsync(id);
        if(reservation is null)
            return new ReservationResponse(false, $"Reservation with id: {id} doesn't exist");

        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();
        return new ReservationResponse(true);
    }

    private bool IsResourceAvailable(Resource resource)
    {
        if (resource.IsAvailable)
        {
            resource.IsAvailable = false;
            return true;
        }
        return false;
    }

    private bool IsReservationNonActive(Reservation reservation) => reservation.EndTime < DateTime.Now;

    private async Task SetStatus()
    {
        var now = DateTime.Now;

        var inactiveReservations = await context.Reservations
            .Include(r => r.Resource)
            .Where(r => r.EndTime < now && r.Status != "inactive")
            .ToListAsync();

        foreach (var reservation in inactiveReservations)
        {
            reservation.Resource.IsAvailable = true;
            reservation.Status = "inactive";
        }

        await context.SaveChangesAsync();
    }
}
