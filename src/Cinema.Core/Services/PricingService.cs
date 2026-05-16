namespace Cinema.Core.Services;

/// <summary>
/// Handles ticket price calculation for cinema reservations.
///
/// The service currently uses a fixed ticket price and applies a 10% group
/// discount when the reservation contains 5 or more tickets.
///
/// This supports the pricing-related requirements:
/// - FR5: Calculate ticket price
/// - FR6: Apply group discount
/// - BR6: A reservation with 5 or more tickets receives a 10% discount
/// </summary>
public class PricingService
{
    private const decimal TicketPrice = 100m;
    private const decimal GroupDiscountRate = 0.10m;
    private const int GroupDiscountThreshold = 5;

    public decimal CalculateTotalPrice(int ticketCount)
    {
        if (ticketCount <= 0)
            return 0;

        var total = ticketCount * TicketPrice;

        if (ticketCount >= GroupDiscountThreshold)
            total *= 1 - GroupDiscountRate;

        return total;
    }
}