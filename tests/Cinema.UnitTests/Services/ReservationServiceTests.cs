using Cinema.Core.Entities;
using Cinema.Core.Services;
using Cinema.UnitTests.Fakes;

namespace Cinema.UnitTests.Services;

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

    /// <summary>
    /// Age
    /// </summary>
    [Fact]
    public void ReserveSeats_BB_AGE_01_CustomerYoungerThanMovieRating_ShouldRejectReservation()
    {
        // Arrange
        var screening = CreateDefaultScreening(movieAgeRating: 15);
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
        var screening = CreateDefaultScreening(movieAgeRating: 15);
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
        var screening = CreateDefaultScreening(movieAgeRating: 15);
        var requestedSeats = new List<string> { "A1" };
        var customerAge = 16;

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Reservation);
    }
    
    /// <summary>
    /// Seat
    /// </summary>
    [Fact]
    public void ReserveSeats_BB_SEAT_01_SeatExistsAndIsAvailable_ShouldAcceptReservation()
    {
        // Arrange
        var screening = CreateDefaultScreening();
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
        var screening = CreateDefaultScreening();
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
        var screening = CreateDefaultScreening();
        var requestedSeats = new List<string> { "Z99" };

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("SEAT_DOES_NOT_EXIST", result.ErrorCode);
    }

    /// <summary>
    /// Time
    /// </summary>
    [Fact]
    public void ReserveSeats_BB_TIME_01_BeforeScreeningStart_ShouldAcceptReservation()
    {
        // Arrange
        var screening = CreateDefaultScreening();
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
        var screening = CreateDefaultScreening();
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
        var screening = CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 20, 1, 0);

        var requestedSeats = new List<string> { "A1" };

        // Act
        var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("SCREENING_ALREADY_STARTED", result.ErrorCode);
    }
    
    /// <summary>
    /// Cancel
    /// </summary>
    [Fact]
    public void CancelReservation_BB_CANCEL_01_MoreThanTwoHoursBeforeScreening_ShouldAcceptCancellation()
    {
        // Arrange
        var screening = CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 17, 59, 0);

        var reservation = CreateDefaultReservation(screening.Id, "A1");
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
        var screening = CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 18, 0, 0);

        var reservation = CreateDefaultReservation(screening.Id, "A1");
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
        var screening = CreateDefaultScreening();
        screening.StartsAt = new DateTime(2026, 5, 15, 20, 0, 0);

        _timeProvider.Now = new DateTime(2026, 5, 15, 18, 1, 0);

        var reservation = CreateDefaultReservation(screening.Id, "A1");
        screening.ReservedSeats.Add("A1");

        // Act
        var result = _reservationService.CancelReservation(screening, reservation);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("CANCELLATION_TOO_LATE", result.ErrorCode);
        Assert.False(reservation.IsCancelled);
        Assert.Contains("A1", screening.ReservedSeats);
    }

    private static Screening CreateDefaultScreening(int movieAgeRating = 15)
    {
        return new Screening
        {
            Id = Guid.NewGuid(),
            StartsAt = new DateTime(2026, 5, 15, 20, 0, 0),
            Movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Test Movie",
                AgeRating = movieAgeRating
            },
            Seats =
            [
                new Seat { SeatNumber = "A1" },
                new Seat { SeatNumber = "A2" },
                new Seat { SeatNumber = "A3" }
            ],
            ReservedSeats = []
        };
    }

    private static Reservation CreateDefaultReservation(Guid screeningId, params string[] seatNumbers)
    {
        return new Reservation
        {
            Id = Guid.NewGuid(),
            ScreeningId = screeningId,
            SeatNumbers = seatNumbers.ToList(),
            CustomerAge = 18,
            TotalPrice = seatNumbers.Length * 100m,
            IsCancelled = false
        };
    }
}