namespace OrderFlow.Console.Models;

public class OrderItem
{
    private Product Product { get; set; }
    private int Quantity { get; set; } // Zmieniłem na public, żeby TotalPrice działał

    public decimal TotalPrice => Product.Price * Quantity;

    public OrderItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }
}