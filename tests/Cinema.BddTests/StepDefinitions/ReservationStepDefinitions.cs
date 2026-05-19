using Cinema.Core.Entities;
using Cinema.Core.Results;
using Cinema.Core.Services;
using Cinema.UnitTests.Fakes;
using Reqnroll;

namespace Cinema.BddTests.StepDefinitions;

/// <summary>
/// Step definitions for the Reqnroll BDD reservation scenarios.
///
/// These steps translate the Gherkin scenarios in <c>Reservation.feature</c>
/// into executable tests against the real <see cref="ReservationService"/>.
///
/// The scenarios use in-memory domain objects and a <see cref="FakeTimeProvider"/>
/// so the tests remain deterministic and independent from the API layer and
/// the real system clock.
/// </summary>
[Binding]
public class ReservationStepDefinitions
{
    private readonly FakeTimeProvider _timeProvider = new();
    private readonly PricingService _pricingService = new();
    private ReservationService _reservationService = null!;

    private Screening _screening = null!;
    private ReservationResult _result = null!;

    [Given(@"a screening exists with a movie age rating of (.*)")]
    public void GivenAScreeningExistsWithAMovieAgeRatingOf(int ageRating)
    {
        _screening = new Screening
        {
            Id = Guid.NewGuid(),
            StartsAt = new DateTime(2030, 5, 15, 20, 0, 0),
            Movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "BDD Test Movie",
                AgeRating = ageRating
            },
            Seats =
            [
                new Seat { SeatNumber = "A1" },
                new Seat { SeatNumber = "A2" }
            ],
            ReservedSeats = []
        };

        _reservationService = new ReservationService(_timeProvider, _pricingService);
    }

    [Given(@"seat (.*) is available")]
    public void GivenSeatIsAvailable(string seatNumber)
    {
        _screening.ReservedSeats.Remove(seatNumber);
    }

    [Given(@"seat (.*) is already reserved")]
    public void GivenSeatIsAlreadyReserved(string seatNumber)
    {
        if (!_screening.ReservedSeats.Contains(seatNumber))
        {
            _screening.ReservedSeats.Add(seatNumber);
        }
    }

    [Given(@"the current time is before the screening starts")]
    public void GivenTheCurrentTimeIsBeforeTheScreeningStarts()
    {
        _timeProvider.Now = _screening.StartsAt.AddHours(-3);
    }

    [When(@"a customer aged (.*) reserves seat (.*)")]
    public void WhenACustomerAgedReservesSeat(int customerAge, string seatNumber)
    {
        _result = _reservationService.ReserveSeats(_screening, [seatNumber], customerAge);
    }

    [Then(@"the reservation should be accepted")]
    public void ThenTheReservationShouldBeAccepted()
    {
        Assert.True(_result.Success);
        Assert.NotNull(_result.Reservation);
    }

    [Then(@"the reservation should be rejected with error code (.*)")]
    public void ThenTheReservationShouldBeRejectedWithErrorCode(string expectedErrorCode)
    {
        Assert.False(_result.Success);
        Assert.Equal(expectedErrorCode, _result.ErrorCode);
    }
}