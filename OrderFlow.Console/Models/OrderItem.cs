namespace OrderFlow.Console.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
    
    public Product Product { get; set; }
    public int ProductId { get; set; }
    
    public int OrderId { get; set; }
    public Order Order { get; set; }
    

    public OrderItem()
    {
        
    }

    public OrderItem(Product product, int quantity, decimal unitPrice)
    {
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}