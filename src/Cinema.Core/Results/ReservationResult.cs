using Cinema.Core.Entities;

namespace Cinema.Core.Results;

public class ReservationResult
{
    public bool Success { get; set; }
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
    public Reservation? Reservation { get; set; }

    public static ReservationResult Ok(Reservation reservation)
    {
        return new ReservationResult
        {
            Success = true,
            Reservation = reservation
        };
    }

    public static ReservationResult Fail(string errorCode, string message)
    {
        return new ReservationResult
        {
            Success = false,
            ErrorCode = errorCode,
            Message = message
        };
    }
}