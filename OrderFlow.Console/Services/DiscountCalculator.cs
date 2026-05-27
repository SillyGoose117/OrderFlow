using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public class DiscountCalculator
{
    public decimal CalculateDiscount(Order order)
    {
        var discount = 0m;
        if (order.Customer.IsVIP)
        {
            discount = 0.10m;
        }
        discount = order.TotalAmount > 1000 ? discount += 0.05m : discount;
        discount *= order.TotalAmount;
        return discount;
    }
}