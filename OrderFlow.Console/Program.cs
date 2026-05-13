using Microsoft.EntityFrameworkCore;
using OrderFlow.Console.Persistence;
using OrderFlow.Console.Services;
using OrderFlow.Console.Watchers;

namespace OrderFlow.Console;
using Data;
using System.Xml.Linq;
using Models;

class Program

{
    static async Task Main(string[] args)
    {
        //Inicjalizacja bazy danych 
        await using var db = new OrderFlowContext();
        await db.Database.MigrateAsync();
        await DatabaseSeeder.SeedAsync(db);
        
        
        //Początek operacji CRUD - Create:
        var customer1 = await db.Customers.FirstOrDefaultAsync();
        var products = await db.Products.Take(2).ToListAsync();
        var order1 = new Order(customer1);
        var item1 = new OrderItem(products[0], 5, products[0].Price);
        var item2 = new OrderItem(products[1], 2, products[1].Price);
        order1.Items.Add(item1);
        order1.Items.Add(item2);
        db.Orders.Add(order1);
        await db.SaveChangesAsync();
        System.Console.WriteLine($"Order {order1.OrderId} with 2 items has been created.");
        
        //CRUD - Read:
        var everyOrder = await db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();
        System.Console.WriteLine($"There are currently this many total orders stored in the database: {everyOrder.Count}.");
        foreach (var order in everyOrder)
        {
            System.Console.WriteLine($"Order ID: {order.OrderId} | Customer: {order.Customer.FullName} | Items count: {order.Items.Count} | Total: {order.TotalAmount}$");
        }
        
        //CRUD - Update:
        var order2 = await db.Orders.FirstOrDefaultAsync(o => o.CurrentStatus == Order.Status.New);
        if (order2 != null)
        {
            order2.CurrentStatus = Order.Status.Processing;
            order2.Notes = $"Gathering items for this order {order2.OrderId}.";
            await db.SaveChangesAsync();
            System.Console.WriteLine($"This order {order2.OrderId} is now being processed.");
        }
        
        //CRUD - Delete:
        var cancelledOrder = await db.Orders.FirstOrDefaultAsync(o => o.CurrentStatus == Order.Status.Cancelled);
        if (cancelledOrder != null)
        {
            db.Orders.Remove(cancelledOrder);
            await db.SaveChangesAsync();
            System.Console.WriteLine($"Order {cancelledOrder.OrderId} has been deleted.");
        }
        else
        {
            System.Console.WriteLine("No orders found for deletion.");
        }
        
    }
}