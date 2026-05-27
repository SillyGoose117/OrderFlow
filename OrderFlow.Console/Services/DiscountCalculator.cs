using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public class DiscountCalculator
{
    private const decimal VipDiscountRate = 0.10m;
    private const decimal DiscountRate = 0.05m;
    private const decimal DiscountThreshold = 0.25m;
    private const decimal HighValueDiscountThreshold = 10000m;
    private const decimal VipHighValueDiscountThreshold = 5000m;
    private const decimal LowValueDiscountThreshold = 1000m;
    public decimal CalculateDiscount(Order order)
    {
        var discount = CalculateDiscountPercentage(order);
        
        if (discount > DiscountThreshold)
        {
            discount = DiscountThreshold;
        }
        discount *= order.TotalAmount;
        return discount;
    }

    private decimal CalculateDiscountPercentage(Order order)
    {
        decimal discount = 0;
        if (order.Customer.IsVIP) //+15% total
        {
            discount = VipDiscountRate;
            if (order.TotalAmount > VipHighValueDiscountThreshold)
            {
                discount += DiscountRate;
            }
        }
        
        if (order.TotalAmount > LowValueDiscountThreshold) //+10% total
        {
            discount += DiscountRate;
            if (order.TotalAmount > HighValueDiscountThreshold)
            {
                discount += DiscountRate;
            }
        }
        return discount;
    }
}