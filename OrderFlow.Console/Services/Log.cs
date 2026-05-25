using OrderFlow.Console.Events;
using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public class Log
{
    public void LogThisGoodOrder(object? sender, OrderStatusChangedEventArgs e)
    {
        if (e.NewStatus == Order.Status.Completed)
        {
            System.Console.WriteLine($"[Log] Order {e.Order.OrderId} - {e.Timestamp} has been completed!");
        }
    }

    public void LogThisBadOrder(object? sender, OrderValidationEventArgs e)
    {
        if (e.Errors.Count > 0)
        {
            System.Console.WriteLine($"[Log] Order with this id, {e.Order.OrderId} has encountered some unexpected errors:");
            foreach (var error in e.Errors)
            {
                var individualError = error.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                foreach (var errorMessage in individualError)
                {
                    System.Console.WriteLine($"-> {errorMessage}");     
                }
            }
        }
    }
}