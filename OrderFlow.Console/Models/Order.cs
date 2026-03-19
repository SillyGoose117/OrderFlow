namespace OrderFlow.Console.Models;

public class Order
{
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public enum Status { New, Validated, Processing, Completed, Cancelled }
    public Status CurrentStatus { get; set; }
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);

    public Order(Customer customer)
    {
        Customer = customer;
        CurrentStatus = Status.New;
    }
}