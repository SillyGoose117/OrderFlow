namespace OrderFlow.Console.Models;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; }
    public int ProductId { get; set; }
    public List<OrderItem> OrderedItems { get; set; }


    public Product()
    {
        
    }

    public Product(string productName, decimal price, int stock, string category)
    {
        Name = productName;
        Price = price;
        Category = category;
        Stock = stock;
    }

    public override string ToString()
    {
        return $"{Category} - {Name} at  {Price}$. Amount left: {Stock}x.";

    }
}