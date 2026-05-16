namespace Cinema.Core.Entities;

public class Screening
{
    public Guid Id { get; set; }
    public Movie Movie { get; set; } = new();
    public DateTime StartsAt { get; set; }
    public List<Seat> Seats { get; set; } = new();
    public List<string> ReservedSeats { get; set; } = new();
}