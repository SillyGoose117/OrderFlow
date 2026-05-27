using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public class DiscountCalculator
{
    public decimal CalculateDiscount(Order order)
    {
        decimal discount = order.Customer.IsVIP ? order.TotalAmount * 0.10m : 0m;
        return discount;
    }
}