namespace Cinema.Core.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid ScreeningId { get; set; }
    public List<string> SeatNumbers { get; set; } = new();
    public int CustomerAge { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsCancelled { get; set; }
}