namespace OrderFlow.Console.Models;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int AmountLeft { get; set; }
    public string Category { get; set; }

    public Product(string productName, decimal price, int quantityLeft, string category)
    {
        Name = productName;
        Price = price;
        AmountLeft = quantityLeft;
        Category = category;
    }

    public override string ToString()
    {
        return $"{Category} - {Name} at  {Price}$. Amount left: {AmountLeft}x.";
    }
}