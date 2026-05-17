using Cinema.Api.Contracts.Dtos;
using Cinema.Core.Entities;
using Cinema.Core.Results;
using Cinema.Core.Services;
using Cinema.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Cinema.Core.Interfaces;

namespace Cinema.Api.Controllers;

/// <summary>
/// API controller for creating and cancelling cinema reservations.
/// </summary>
[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private readonly InMemoryCinemaStore _store;
    private readonly IReservationService _reservationService;
    
    public ReservationsController(InMemoryCinemaStore store, IReservationService reservationService)
    {
        _store = store;
        _reservationService = reservationService;
    }

    /// <summary>
    /// Creates a reservation for seats in a screening.
    /// </summary>
    [HttpPost]
    public IActionResult CreateReservation(CreateReservationRequest request)
    {   
        var screening = _store.Screenings.FirstOrDefault(s => s.Id == request.ScreeningId);

        if (screening is null)
        {
            return NotFound(new
            {
                errorCode = "SCREENING_NOT_FOUND",
                message = $"Screening {request.ScreeningId} was not found."
            });
        }

        var result = _reservationService.ReserveSeats(screening, request.SeatNumbers, request.CustomerAge);

        if (!result.Success)
        {
            return MapFailureResult(result);
        }

        _store.Reservations.Add(result.Reservation!);

        return CreatedAtAction(
            nameof(GetReservationById),
            new { reservationId = result.Reservation!.Id },
            ToReservationResponse(result.Reservation));
    }

    /// <summary>
    /// Gets a reservation by ID.
    /// </summary>
    [HttpGet("{reservationId:guid}")]
    public IActionResult GetReservationById(Guid reservationId)
    {
        var reservation = _store.Reservations.FirstOrDefault(r => r.Id == reservationId);

        if (reservation is null)
        {
            return NotFound(new
            {
                errorCode = "RESERVATION_NOT_FOUND",
                message = $"Reservation {reservationId} was not found."
            });
        }

        return Ok(ToReservationResponse(reservation));
    }

    /// <summary>
    /// Cancels an existing reservation.
    /// </summary>
    [HttpPost("{reservationId:guid}/cancel")]
    public IActionResult CancelReservation(Guid reservationId)
    {
        var reservation = _store.Reservations.FirstOrDefault(r => r.Id == reservationId);

        if (reservation is null)
        {
            return NotFound(new
            {
                errorCode = "RESERVATION_NOT_FOUND",
                message = $"Reservation {reservationId} was not found."
            });
        }

        var screening = _store.Screenings.FirstOrDefault(s => s.Id == reservation.ScreeningId);

        if (screening is null)
        {
            return NotFound(new
            {
                errorCode = "SCREENING_NOT_FOUND",
                message = $"Screening {reservation.ScreeningId} was not found."
            });
        }

        var result = _reservationService.CancelReservation(screening, reservation);

        if (!result.Success)
        {
            return MapFailureResult(result);
        }

        return Ok(new CancelReservationResponse
        {
            ReservationId = reservation.Id,
            IsCancelled = reservation.IsCancelled
        });
    }

    // Maps core service failures to HTTP responses.
    private IActionResult MapFailureResult(ReservationResult result)
    {
        var errorResponse = new
        {
            errorCode = result.ErrorCode,
            message = result.Message
        };

        return result.ErrorCode switch
        {
            "NO_SEATS_REQUESTED" => BadRequest(errorResponse),
            "SCREENING_ALREADY_STARTED" => Conflict(errorResponse),
            "CUSTOMER_TOO_YOUNG" => BadRequest(errorResponse),
            "SEAT_DOES_NOT_EXIST" => BadRequest(errorResponse),
            "SEAT_ALREADY_RESERVED" => Conflict(errorResponse),
            "CANCELLATION_TOO_LATE" => Conflict(errorResponse),
            _ => BadRequest(errorResponse)
        };
    }

    private static ReservationResponse ToReservationResponse(Reservation reservation)
    {
        return new ReservationResponse
        {
            ReservationId = reservation.Id,
            ScreeningId = reservation.ScreeningId,
            SeatNumbers = reservation.SeatNumbers,
            CustomerAge = reservation.CustomerAge,
            TotalPrice = reservation.TotalPrice,
            IsCancelled = reservation.IsCancelled
        };
    }
}