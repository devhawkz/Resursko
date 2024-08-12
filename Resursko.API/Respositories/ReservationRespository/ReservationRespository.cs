using Microsoft.AspNetCore.Identity;
using Resursko.API.Services.EmailService;
using Resursko.API.Services.UserContext;
using Resursko.Domain.DTOs.ReservationDTO;
using Resursko.Domain.Models;

namespace Resursko.API.Respositories.ReservationRespository;

public class ReservationRespository(DataContext context, IUserContextService userContextService, IEmailSenderAsync emailSender) : IReservationRespository
{
    private static List<Reservation> _reservations = new List<Reservation>();
    private static bool _isListChanged = false;

    public async Task<ReservationResponse> CreateNewReservation(Reservation reservation)
    {
        var userId = userContextService.GetUserId();
        if (userId is null)
            return new ReservationResponse(false, "You must log in to perform this action!");

        if(reservation.StartTime < DateTime.Now)
            return new ReservationResponse(false, "Start time can not be in the past.");

        if (reservation.StartTime > reservation.EndTime)
            return new ReservationResponse(false, "Start time can't be older than end time");

        reservation.Resource = await context.Resources.FindAsync(reservation.ResourceId);
        reservation.User = await context.Users.FindAsync(userId);

        if (!await IsResourceAvailable(reservation))
            return new ReservationResponse(false, "Resource is not available in selected period.");

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();
        _isListChanged = true;

        var message = new Message(new string[] { userContextService.GetUserEmail()! }, "Reservation created", $"You have successfully created reservation with id {reservation.Id}");

        await emailSender.SendEmailAsync(message);

        return new ReservationResponse(true);
    }

    public async Task<List<GetAllReservationResponse>> GetAllReservations()
    {
        _isListChanged = false;
        await GetAllReservationsFromDb();
        await Reminder();

        return _reservations
             .Select(r => new GetAllReservationResponse
             {
                 Username = r.User.UserName,
                 Email = r.User.Email,
                 ResourceName = r.Resource.Name,
                 StartTime = r.StartTime,
                 EndTime = r.EndTime,
                 Status = r.Status
             })
             .ToList();
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

        if(!await IsResourceAvailable(reservation))
            return new ReservationResponse(false, "Resource is not available in selected period.");

        dbReservation.StartTime = reservation.StartTime;
        dbReservation.EndTime = reservation.EndTime;
        dbReservation.Status = reservation.Status;
        dbReservation.Resource =  resource!;

        await context.SaveChangesAsync();
        _isListChanged = true;

        var message = new Message(new string[] { userContextService.GetUserEmail()! }, "Reservation update", $"You have successfully update reservation with id {id}");

        await emailSender.SendEmailAsync(message);

        return new ReservationResponse(true);
    }

    public async Task<ReservationResponse> DeleteReservation(int id)
    {
        var reservation = await context.Reservations.FindAsync(id);
        if(reservation is null)
            return new ReservationResponse(false, $"Reservation with id: {id} doesn't exist");
        
        // removing reservation
        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();

        var message = new Message(new string[] { userContextService.GetUserEmail()! }, "Reservation canceled", $"You have successfully canceled reservation with id {reservation.Id}");
        await emailSender.SendEmailAsync(message);

        return new ReservationResponse(true);
    }

    public async Task<List<GetAllReservationResponse>> GetReservationsByCurrentUser()
    {
        var userId = userContextService.GetUserId();

        if (_isListChanged)
            _reservations = await GetAllReservationsFromDb();

        return _reservations.Where(r => r.UserId == userId)
            .Select(r => new GetAllReservationResponse
            {
                Username = r.User.UserName,
                Email = r.User.Email,
                ResourceName = r.Resource.Name,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status
            })
            .ToList();
    }

    private async Task<bool> IsResourceAvailable(Reservation reservation)
    {
        if (_isListChanged)
            _reservations = await GetAllReservationsFromDb();

        var activeReservations = _reservations
            .Where(r => r.ResourceId == reservation.ResourceId && r.Status == "active")
            .OrderBy(r => r.StartTime)
            .ToList();

        foreach (var activeReservation in activeReservations)
        {
            if (reservation.StartTime < activeReservation.EndTime && reservation.EndTime > activeReservation.StartTime)
                return false;
        }

        return true;
    }

    private async Task<List<Reservation>> GetAllReservationsFromDb()
    {
        _reservations = await context.Reservations
           .Include(r => r.User)
           .Include(r => r.Resource)
           .ToListAsync();

        return _reservations;
    }

    private async Task Reminder()
    {
        var activeReservations = await context.Reservations
            .Include(r => r.User)
            .Where(r => r.Status == "active")
            .ToListAsync();

        foreach (var reservation in activeReservations)
        {
            TimeSpan difference = reservation.StartTime - DateTime.Now;

            if (difference <= TimeSpan.FromDays(1))
            {
                var message = new Message(new string[] { reservation.User.Email! }, "Reservation reminder", $"You have reservation with id {reservation.Id} that begins in one day");
                await emailSender.SendEmailAsync(message);
            }
        }

        await context.SaveChangesAsync();
    }

   
}
