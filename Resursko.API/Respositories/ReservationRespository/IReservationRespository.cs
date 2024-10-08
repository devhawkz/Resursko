﻿using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Respositories.ReservationRespository;

public interface IReservationRespository
{
    Task<ReservationResponse> CreateNewReservation(Reservation reservation);
    Task<List<GetAllReservationResponse>> GetAllReservations();
    Task<ReservationResponse> UpdateReservation(Reservation reservation, int id);
    Task<ReservationResponse> DeleteReservation(int id);
    Task<List<GetAllReservationResponse>> GetReservationsByResource(int id);
}
