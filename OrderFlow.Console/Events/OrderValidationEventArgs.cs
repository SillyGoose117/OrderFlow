namespace OrderFlow.Console.Events;
using Models;

public class OrderValidationEventArgs : EventArgs
{
    public Order Order { get; }
    public bool IsValid { get; }
    public List<string> Errors { get; }

    public OrderValidationEventArgs(Order order, bool isValid, List<string> errors)
    {
        Order = order;
        IsValid = isValid;
        Errors = errors;
    }
}