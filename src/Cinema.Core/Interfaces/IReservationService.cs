using Cinema.Core.Entities;
using Cinema.Core.Results;

namespace Cinema.Core.Interfaces;

public interface IReservationService
{
    ReservationResult ReserveSeats(Screening screening, List<string>? requestedSeats, int customerAge);
    ReservationResult CancelReservation(Screening screening, Reservation reservation);
}