namespace OrderFlow.Console.Models;

public class OrderItem
{
    public Product product { get; set; }
    public int amountOrdered { get; set; }

    public decimal TotalPrice => product.Price * amountOrdered;

    public OrderItem()
    {
        
    }

    public OrderItem(Product product, int quantity)
    {
        this.product = product;
        this.amountOrdered = quantity;
    }
}