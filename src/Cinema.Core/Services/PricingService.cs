namespace Cinema.Core.Services;

/// <summary>
/// Responsible for calculating the total price for a reservation for a screening
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