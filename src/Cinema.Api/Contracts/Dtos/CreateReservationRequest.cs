namespace Cinema.Api.Contracts.Dtos;


/// <summary>
/// Request body used when creating a cinema reservation.
/// </summary>
public class CreateReservationRequest
{
    public Guid ScreeningId { get; set; }

    public List<string> SeatNumbers { get; set; } = [];

    public int CustomerAge { get; set; }
}