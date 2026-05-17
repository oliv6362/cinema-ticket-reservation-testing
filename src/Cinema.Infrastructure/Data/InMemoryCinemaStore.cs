using Cinema.Core.Entities;

namespace Cinema.Infrastructure.Data;

/// <summary>
/// Simple in-memory data store used by the API layer during development
/// and API testing.
/// </summary>
public class InMemoryCinemaStore
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
        var screening = new Screening
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            StartsAt = new DateTime(2026, 5, 20, 20, 0, 0),
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

        Screenings.Add(screening);
    }
}