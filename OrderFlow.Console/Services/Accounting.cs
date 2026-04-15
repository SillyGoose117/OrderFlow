using OrderFlow.Console.Events;

namespace OrderFlow.Console.Services;

public class Accounting
{
    public void GoodAccount(object? sender, OrderStatusChangedEventArgs e)
    {
        System.Console.WriteLine($"[Accounting] Notice of this order's status change from {e.OldStatus} to {e.NewStatus}.");
    }

    // public void BadAccount(object? sender, OrderValidationEventArgs e)
    // {
    //     System.Console.WriteLine($"[Accounting] There was a problem while validating the order {e.Order.OrderId}.");
    // }
}