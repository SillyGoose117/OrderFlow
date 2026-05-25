using OrderFlow.Console.Events;
using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public class OrderPipeline
{
    public event EventHandler<OrderStatusChangedEventArgs>? StatusChanged;
    public event EventHandler<OrderValidationEventArgs>? ValidationCompleted;

    //Połączone ze wcześniej przygotowaną klasą "OrderValidator"
    //Wykorzystanie klasy jako warunek dla "OrderValidationEventArgs"
    public void ProcessOrder(Order order)
    {
        System.Console.WriteLine("Beginning order validation...");
        var validator = new OrderValidator(order);
        var errors = validator.ValidateAll();
        bool isValid = errors.Count == 0;
        ValidationCompleted?.Invoke(this, new OrderValidationEventArgs(order, isValid, errors));
        
        if (!isValid) return;
        System.Console.WriteLine("Order validated, beginning processing...");
        //Z tego co zrozumiałem 1 uruchomienie skuutkuje przejściem przez wszystkie procesy zamawiania
        StatusUpdater(order, Order.Status.Validated);
        StatusUpdater(order, Order.Status.Processing);
        StatusUpdater(order, Order.Status.Completed);
        System.Console.WriteLine("Order processing completed, ending processing...");
    }

    private void StatusUpdater (Order order, Order.Status newStatus)
    {
        var oldStatus = order.CurrentStatus;
        order.CurrentStatus = newStatus;
        StatusChanged?.Invoke(this, new OrderStatusChangedEventArgs(order, oldStatus, newStatus));
    }
}