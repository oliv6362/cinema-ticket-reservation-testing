using Cinema.UnitTests.TestData;
using Cinema.Core.Services;
using Cinema.UnitTests.Fakes;

namespace Cinema.UnitTests.Services;

/// <summary>
/// Unit tests for <see cref="ReservationService"/>.
///
/// These tests verify the reservation and cancellation rules from the
/// Cinema Ticket Reservation System requirements.
///
/// The tests are derived from the black-box test design:
/// - FR2: Reject unavailable seats
/// - FR3: Validate age restrictions
/// - FR4: Validate reservation time
/// - FR7: Cancel reservation
/// - FR8: Reject late cancellation
/// - BR1: Customer age must be greater than or equal to the movie age rating
/// - BR2: All requested seats must exist in the screening room
/// - BR3: All requested seats must be available for the selected screening
/// - BR5: A reservation can only be created before the screening starts
/// - BR7: A reservation can only be cancelled at least 2 hours before the screening starts
///
/// Boundary value testing is used for age restrictions, reservation time,
/// and cancellation deadline. Equivalence partitioning is used for seat
/// existence and availability.
///
/// The BB prefix in the test names refers to black-box test cases from
/// the black-box test design document.
/// </summary>
public class ReservationServiceTests
{
    private readonly FakeTimeProvider _timeProvider;
    private readonly PricingService _pricingService;
    private readonly ReservationService _reservationService;

    public ReservationServiceTests()
    {
        _timeProvider = new FakeTimeProvider
        {
            Now = new DateTime(2026, 5, 15, 17, 0, 0)
        };

        _pricingService = new PricingService();
        _reservationService = new ReservationService(_timeProvider, _pricingService);
    }

    // Age restriction
    [Fact]
    public void ReserveSeats_BB_AGE_01_CustomerYoungerThanMovieRating_ShouldRejectReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening(movieAgeRating: 15);
        var requestedSeats = new List<string> { "A1" };
        var customerAge = 14;

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("CUSTOMER_TOO_YOUNG", result.ErrorCode);
    }

    [Fact]
    public void ReserveSeats_BB_AGE_02_CustomerEqualToMovieRating_ShouldAcceptReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening(movieAgeRating: 15);
        var requestedSeats = new List<string> { "A1" };
        var customerAge = 15;

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Reservation);
    }

    [Fact]
    public void ReserveSeats_BB_AGE_03_CustomerOlderThanMovieRating_ShouldAcceptReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening(movieAgeRating: 15);
        var requestedSeats = new List<string> { "A1" };
        var customerAge = 16;

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Reservation);
    }
    
    // Seat availability
    [Fact]
    public void ReserveSeats_BB_SEAT_01_SeatExistsAndIsAvailable_ShouldAcceptReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        var requestedSeats = new List<string> { "A1" };

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

        // Assert
        Assert.True(result.Success);
        Assert.Contains("A1", screening.ReservedSeats);
    }

    [Fact]
    public void ReserveSeats_BB_SEAT_02_SeatExistsButAlreadyReserved_ShouldRejectReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        screening.ReservedSeats.Add("A2");

        var requestedSeats = new List<string> { "A2" };

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("SEAT_ALREADY_RESERVED", result.ErrorCode);
    }

    [Fact]
    public void ReserveSeats_BB_SEAT_03_SeatDoesNotExist_ShouldRejectReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        var requestedSeats = new List<string> { "Z99" };

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("SEAT_DOES_NOT_EXIST", result.ErrorCode);
    }

    // Reservation time
    [Fact]
    public void ReserveSeats_BB_TIME_01_BeforeScreeningStart_ShouldAcceptReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 19, 59, 0);

        var requestedSeats = new List<string> { "A1" };

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Reservation);
    }

    [Fact]
    public void ReserveSeats_BB_TIME_02_ExactlyAtScreeningStart_ShouldRejectReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 20, 0, 0);

        var requestedSeats = new List<string> { "A1" };

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("SCREENING_ALREADY_STARTED", result.ErrorCode);
    }

    [Fact]
    public void ReserveSeats_BB_TIME_03_AfterScreeningStart_ShouldRejectReservation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 20, 1, 0);

        var requestedSeats = new List<string> { "A1" };

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("SCREENING_ALREADY_STARTED", result.ErrorCode);
    }
    
    // Cancellation deadline
    [Fact]
    public void CancelReservation_BB_CANCEL_01_MoreThanTwoHoursBeforeScreening_ShouldAcceptCancellation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 17, 59, 0);

        var reservation = TestCinemaData.CreateDefaultReservation(screening.Id, "A1");
        screening.ReservedSeats.Add("A1");

        // Act
        var result = _reservationService.CancelReservation(screening, reservation);

        // Assert
        Assert.True(result.Success);
        Assert.True(reservation.IsCancelled);
        Assert.DoesNotContain("A1", screening.ReservedSeats);
    }

    [Fact]
    public void CancelReservation_BB_CANCEL_02_ExactlyTwoHoursBeforeScreening_ShouldAcceptCancellation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 18, 0, 0);

        var reservation = TestCinemaData.CreateDefaultReservation(screening.Id, "A1");
        screening.ReservedSeats.Add("A1");

        // Act
        var result = _reservationService.CancelReservation(screening, reservation);

        // Assert
        Assert.True(result.Success);
        Assert.True(reservation.IsCancelled);
        Assert.DoesNotContain("A1", screening.ReservedSeats);
    }

    [Fact]
    public void CancelReservation_BB_CANCEL_03_LessThanTwoHoursBeforeScreening_ShouldRejectCancellation()
    {
        // Arrange
        var screening = TestCinemaData.CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 18, 1, 0);

        var reservation = TestCinemaData.CreateDefaultReservation(screening.Id, "A1");
        screening.ReservedSeats.Add("A1");

        // Act
        var result = _reservationService.CancelReservation(screening, reservation);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("CANCELLATION_TOO_LATE", result.ErrorCode);
        Assert.False(reservation.IsCancelled);
        Assert.Contains("A1", screening.ReservedSeats);
    }
}