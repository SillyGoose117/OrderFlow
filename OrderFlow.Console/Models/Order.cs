using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace OrderFlow.Console.Models;

public class Order
{
    [XmlElement("customer")]
    public Customer Customer { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public enum Status { New, Validated, Processing, Completed, Cancelled }
    public Status CurrentStatus { get; set; }
    [JsonIgnore] [XmlIgnore]
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
    [XmlAttribute("id")]
    public Guid OrderId { get; set; }

    public Order()
    {
        
    }

    public Order(Customer customer)
    {
        Customer = customer;
        CurrentStatus = Status.New;
        OrderDate = DateTime.Now;
        OrderId =  Guid.NewGuid();
    }

    public override string ToString()
    {
        return $"{OrderId} - {Customer} This order is currently: {CurrentStatus} - {OrderDate}";
    }
}