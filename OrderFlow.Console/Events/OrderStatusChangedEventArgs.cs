namespace OrderFlow.Console.Events;
using Models;

public class OrderStatusChangedEventArgs : EventArgs
{
    public Order Order { get; }
    public Order.Status OldStatus { get; }
    public Order.Status NewStatus { get; }
    public DateTime Timestamp { get; } = DateTime.Now;

    public OrderStatusChangedEventArgs(Order order, Order.Status oldStatus, Order.Status newStatus)
    {
        Order = order;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}