using OrderFlow.Console.Events;
using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public class Warehouse
{
    public void OnStatusChanged(object? sender, OrderStatusChangedEventArgs e)
    {
        if (e.NewStatus == Order.Status.Processing)
        {
            System.Console.WriteLine($"[Warehouse] Order {e.Order.OrderId} is being packed for delivery!");
        }
    }

    // public void OnNoStatusChanged(object? sender, OrderValidationEventArgs e)
    // {
    //     
    // }
}