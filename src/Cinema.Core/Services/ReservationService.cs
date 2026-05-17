using Cinema.Core.Entities;
using Cinema.Core.Interfaces;
using Cinema.Core.Results;

namespace Cinema.Core.Services;

/// <summary>
/// Handles the core business logic for creating and cancelling cinema reservations.
///
/// This service enforces the main reservation rules from the requirements:
/// - A reservation must contain at least one seat.
/// - Reservations cannot be made after the screening has started.
/// - Customers must satisfy the movie age rating.
/// - Requested seats must exist and be available.
/// - Cancellations must be made at least 2 hours before the screening starts.
///
/// The service depends on <see cref="ITimeProvider"/> instead of using
/// <see cref="DateTime.Now"/> directly, which makes time-dependent logic
/// deterministic and unit testable.
/// </summary>
public class ReservationService
{
    private readonly ITimeProvider _timeProvider;
    private readonly PricingService _pricingService;

    public ReservationService(ITimeProvider timeProvider, PricingService pricingService)
    {
        _timeProvider = timeProvider;
        _pricingService = pricingService;
    }

    // Attempts to reserve seats for a screening.
    public ReservationResult ReserveSeats(Screening screening, List<string> requestedSeats, int customerAge)
    {
        if (requestedSeats is null || requestedSeats.Count == 0)
        {
            return ReservationResult.Fail(
                "NO_SEATS_REQUESTED",
                "A reservation must contain at least one seat.");
        }

        if (_timeProvider.Now >= screening.StartsAt)
        {
            return ReservationResult.Fail(
                "SCREENING_ALREADY_STARTED",
                "Reservations cannot be made after the screening has started.");
        }

        if (customerAge < screening.Movie.AgeRating)
        {
            return ReservationResult.Fail(
                "CUSTOMER_TOO_YOUNG",
                "Customer age is below the movie age rating.");
        }

        var existingSeatNumbers = screening.Seats.Select(s => s.SeatNumber).ToHashSet();

        foreach (var seat in requestedSeats)
        {
            if (!existingSeatNumbers.Contains(seat))
            {
                return ReservationResult.Fail(
                    "SEAT_DOES_NOT_EXIST",
                    $"Seat {seat} does not exist.");
            }

            if (screening.ReservedSeats.Contains(seat))
            {
                return ReservationResult.Fail(
                    "SEAT_ALREADY_RESERVED",
                    $"Seat {seat} is already reserved.");
            }
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            ScreeningId = screening.Id,
            SeatNumbers = requestedSeats,
            CustomerAge = customerAge,
            TotalPrice = _pricingService.CalculateTotalPrice(requestedSeats.Count)
        };

        foreach (var seat in requestedSeats)
        {
            screening.ReservedSeats.Add(seat);
        }

        return ReservationResult.Ok(reservation);
    }

    // Attempts to cancel an existing reservation.
    public ReservationResult CancelReservation(Screening screening, Reservation reservation)
    {
        var timeUntilScreening = screening.StartsAt - _timeProvider.Now;

        if (timeUntilScreening < TimeSpan.FromHours(2))
        {
            return ReservationResult.Fail(
                "CANCELLATION_TOO_LATE",
                "Reservation cannot be cancelled less than 2 hours before the screening starts.");
        }

        reservation.IsCancelled = true;

        foreach (var seat in reservation.SeatNumbers)
        {
            screening.ReservedSeats.Remove(seat);
        }

        return ReservationResult.Ok(reservation);
    }
}