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
            discount = order.TotalAmount > 5000 ? discount += 0.05m : discount;
        }
        discount = order.TotalAmount > 1000 ? discount += 0.05m : discount;
        discount = order.TotalAmount > 10000 ? discount += 0.1m : discount; //Dobija do 25% rabatu (nawet więcej bo 30%)
        discount = Math.Min(discount, 0.25m); //Ogranicza rabat do maksymalnnie 25% na potrzeby testu
        discount *= order.TotalAmount;
        return discount;
    }
}