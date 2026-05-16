using Cinema.Core.Services;

namespace Cinema.UnitTests.Services;

/// <summary>
/// Unit tests for <see cref="PricingService"/>.
///
/// These tests verify the ticket price calculation rules from the
/// Cinema Ticket Reservation System requirements.
///
/// The tests are derived from the black-box test design:
/// - FR5: Calculate ticket price
/// - FR6: Apply group discount
/// - BR6: A reservation with 5 or more tickets receives a 10% discount
///
/// Boundary value testing is used because the group discount has a clear
/// numeric boundary at 5 tickets.
///
/// The BB prefix in the test names refers to black-box test cases from
/// the black-box test design document.
/// </summary>
public class PricingServiceTests
{
    [Fact]
    public void CalculateTotalPrice_BB_DISCOUNT_01_FourTickets_ShouldNotApplyGroupDiscount()
    {
        // Arrange
        var pricingService = new PricingService();

        // Act
        var totalPrice = pricingService.CalculateTotalPrice(ticketCount: 4);

        // Assert
        Assert.Equal(400m, totalPrice);
    }

    [Fact]
    public void CalculateTotalPrice_BB_DISCOUNT_02_FiveTickets_ShouldApplyGroupDiscount()
    {
        // Arrange
        var pricingService = new PricingService();

        // Act
        var totalPrice = pricingService.CalculateTotalPrice(ticketCount: 5);

        // Assert
        Assert.Equal(450m, totalPrice);
    }

    [Fact]
    public void CalculateTotalPrice_BB_DISCOUNT_03_SixTickets_ShouldApplyGroupDiscount()
    {
        // Arrange
        var pricingService = new PricingService();

        // Act
        var totalPrice = pricingService.CalculateTotalPrice(ticketCount: 6);

        // Assert
        Assert.Equal(540m, totalPrice);
    }
}