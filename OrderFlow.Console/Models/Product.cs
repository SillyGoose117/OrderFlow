namespace OrderFlow.Console.Models;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }

    public Product(string productName, decimal price, int quantity, string category)
    {
        Name = productName;
        Price = price;
        Quantity = quantity;
        Category = category;
    }
}