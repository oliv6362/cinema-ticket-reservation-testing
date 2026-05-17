namespace Cinema.Api.Contracts.Dtos;

/// <summary>
/// Response returned when a reservation is successfully created.
/// </summary>
public class ReservationResponse
{
    public Guid ReservationId { get; set; }

    public Guid ScreeningId { get; set; }

    public List<string> SeatNumbers { get; set; } = [];

    public int CustomerAge { get; set; }

    public decimal TotalPrice { get; set; }

    public bool IsCancelled { get; set; }
}