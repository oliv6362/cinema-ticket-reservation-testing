using Cinema.Core.Entities;
using Cinema.Core.Interfaces;

namespace Cinema.Infrastructure.Data;

/// <summary>
/// Simple in-memory data store used by the API layer during development
/// and API testing.
/// </summary>
public class InMemoryCinemaStore : ICinemaStore
{
    public List<Screening> Screenings { get; } = [];
    public List<Reservation> Reservations { get; } = [];

    public InMemoryCinemaStore()
    {
        Reset();
    }

    public void Reset()
    {
        Screenings.Clear();
        Reservations.Clear();

        SeedScreenings();
    }

    private void SeedScreenings()
    {
        var futureScreening = new Screening
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            StartsAt = new DateTime(2030, 5, 15, 20, 0, 0),
            Movie = new Movie
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "The Test Movie",
                AgeRating = 15
            },
            Seats =
            [
                new Seat { SeatNumber = "A1" },
                new Seat { SeatNumber = "A2" },
                new Seat { SeatNumber = "A3" },
                new Seat { SeatNumber = "A4" },
                new Seat { SeatNumber = "A5" },
                new Seat { SeatNumber = "A6" }
            ],
            ReservedSeats = []
        };

        var lateCancellationScreening = new Screening
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            StartsAt = DateTime.Now.AddHours(1),
            Movie = new Movie
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Title = "Late Cancellation Test Movie",
                AgeRating = 15
            },
            Seats =
            [
                new Seat { SeatNumber = "B1" },
                new Seat { SeatNumber = "B2" }
            ],
            ReservedSeats = []
        };

        Screenings.Add(futureScreening);
        Screenings.Add(lateCancellationScreening);
    }
}