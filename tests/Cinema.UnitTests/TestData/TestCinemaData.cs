using Cinema.Core.Entities;

namespace Cinema.UnitTests.TestData;

/// <summary>
/// Provides reusable test data for unit tests.
///
/// This helper reduces duplication in test classes by creating common
/// cinema domain objects such as screenings and reservations.
///
/// The default screening is configured with:
/// - Movie age rating: 15
/// - Screening start time: 15 May 2026 at 20:00
/// - Seats: A1, A2, A3
/// - No reserved seats
///
/// Individual tests can modify the returned objects when they need
/// a specific test scenario.
/// </summary>
public static class TestCinemaData
{
    public static Screening CreateDefaultScreening(int movieAgeRating = 15)
    {
        return new Screening
        {
            Id = Guid.NewGuid(),
            StartsAt = new DateTime(2026, 5, 15, 20, 0, 0),
            Movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Test Movie",
                AgeRating = movieAgeRating
            },
            Seats =
            [
                new Seat { SeatNumber = "A1" },
                new Seat { SeatNumber = "A2" },
                new Seat { SeatNumber = "A3" }
            ],
            ReservedSeats = []
        };
    }

    public static Reservation CreateDefaultReservation(Guid screeningId, params string[] seatNumbers)
    {
        return new Reservation
        {
            Id = Guid.NewGuid(),
            ScreeningId = screeningId,
            SeatNumbers = seatNumbers.ToList(),
            CustomerAge = 18,
            TotalPrice = seatNumbers.Length * 100m,
            IsCancelled = false
        };
    }
}