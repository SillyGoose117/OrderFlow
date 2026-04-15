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
        if (e.Errors.Any())
        {
            System.Console.WriteLine($"[Log] Order with this id, {e.Order.OrderId} has encountered some unexpected errors:");
            foreach (var error in e.Errors)
            {
             System.Console.WriteLine($"->{error}");   
            }
        }
    }
}