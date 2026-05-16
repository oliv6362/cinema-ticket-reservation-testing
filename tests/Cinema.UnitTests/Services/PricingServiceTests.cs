using Cinema.Core.Services;

namespace Cinema.UnitTests.Services;

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