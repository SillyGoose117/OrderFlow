using Microsoft.EntityFrameworkCore;
using OrderFlow.Console.Data;

namespace OrderFlow.Console.Persistence;

public class DatabaseSeeder
{
    public static async Task SeedAsync(OrderFlowContext db)
    {
        if (await db.Orders.AnyAsync()) //Jeżeli są zamówienia na podstawie innych danych to nie ma sensu ich sprawdzać oddzielnie
            return;

        var products = SampleData.ProductList;
        db.Products.AddRange(products);
        
        var customers = SampleData.ListOfCustomers;
        db.Customers.AddRange(customers);
        
        var orders = SampleData.ListOfOrders;
        db.Orders.AddRange(orders);
        
        await db.SaveChangesAsync();
    }
}