using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace OrderFlow.Console.Models;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public enum Status { New, Validated, Processing, Completed, Cancelled }
    public Status CurrentStatus { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
    public string? Notes { get; set; }

    public Order()
    {
        
    }

    public Order(Customer customer)
    {
        Customer = customer;
        CurrentStatus = Status.New;
        OrderDate = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{OrderId} - {Customer} This order is currently: {CurrentStatus} - {OrderDate}";
    }
}