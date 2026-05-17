namespace Cinema.Api.Contracts.Dtos;

/// <summary>
/// Response returned when a reservation is successfully cancelled.
/// </summary>
public class CancelReservationResponse
{
    public Guid ReservationId { get; set; }

    public bool IsCancelled { get; set; }
}