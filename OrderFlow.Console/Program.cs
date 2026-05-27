using Microsoft.EntityFrameworkCore;
using OrderFlow.Console.Data;
using OrderFlow.Console.Persistence;

namespace OrderFlow.Console;
using Models;

class Program

{
    static async Task Main(string[] args)
    {
    //     //     //Inicjalizacja bazy danych
    //     await using var db = new OrderFlowContext();
    //     await db.Database.MigrateAsync();
    //     await DatabaseSeeder.SeedAsync(db);
    //     
    //     System.Console.WriteLine("================================");
    //     System.Console.WriteLine(" Zadanie 1 ");
    //     System.Console.WriteLine("================================");
    //
    //     var migrations = await db.Database.GetAppliedMigrationsAsync();
    //     foreach (var migration in migrations)
    //     {
    //         System.Console.WriteLine(migration);   
    //     }
    //
    //     System.Console.WriteLine("================================");
    //     System.Console.WriteLine(" Zadanie 2 ");
    //     System.Console.WriteLine("================================");
    //     
    //     //Początek operacji CRUD - Create:
    //     var customer1 = await db.Customers.FirstOrDefaultAsync();
    //     var products = await db.Products.Take(2).ToListAsync();
    //     var order1 = new Order(customer1);
    //     var item1 = new OrderItem(products[0], 5, products[0].Price);
    //     var item2 = new OrderItem(products[1], 2, products[1].Price);
    //     order1.Items.Add(item1);
    //     order1.Items.Add(item2);
    //     db.Orders.Add(order1);
    //     await db.SaveChangesAsync();
    //     System.Console.WriteLine($"Order {order1.OrderId} with 2 items has been created.");
    //     
    //     //CRUD - Read:
    //     var everyOrder = await db.Orders
    //         .Include(o => o.Customer)
    //         .Include(o => o.Items)
    //         .ThenInclude(i => i.Product)
    //         .ToListAsync();
    //     System.Console.WriteLine($"There are currently this many total orders stored in the database: {everyOrder.Count}.");
    //     foreach (var order in everyOrder) 
    //     { 
    //         System.Console.WriteLine($"Order ID: {order.OrderId} | Customer: {order.Customer.FullName} | Items count: {order.Items.Count} | Total: {order.TotalAmount}$"); 
    //     }
    //     
    //     //CRUD - Update:
    //     var order2 = await db.Orders.FirstOrDefaultAsync(o => o.CurrentStatus == Order.Status.New);
    //     if (order2 != null)
    //     {
    //         order2.CurrentStatus = Order.Status.Processing;
    //         order2.Notes = $"Gathering items for this order {order2.OrderId}.";
    //         await db.SaveChangesAsync();
    //         System.Console.WriteLine($"This order {order2.OrderId} is now being processed.");
    //     }
    //     
    //     //CRUD - Delete:
    //     var cancelledOrder = await db.Orders.FirstOrDefaultAsync(o => o.CurrentStatus == Order.Status.Cancelled);
    //     if (cancelledOrder != null)
    //     {
    //         db.Orders.Remove(cancelledOrder);
    //         await db.SaveChangesAsync();
    //         System.Console.WriteLine($"Order {cancelledOrder.OrderId} has been deleted.");
    //     }
    //     else
    //     {
    //         System.Console.WriteLine("No orders found for deletion.");
    //     }
    //
    //     System.Console.WriteLine("================================");
    //     System.Console.WriteLine(" Zadanie 3 ");
    //     System.Console.WriteLine("================================");
    //
    //      //Linq
    //      //Zamówienia klientów VIP z kwotą powyżej zadanego progu (Include lub Select z nawigacją).
    //      decimal threshold = 100m;
    //      var vipOrders = await db.Orders
    //          .Where(o => o.Customer.IsVIP && o.Items.Sum(i => i.Quantity * i.UnitPrice) > threshold)
    //          .Select(o => new
    //          {
    //              o.OrderId, 
    //              o.Customer.FullName,
    //              TotalPrice = o.Items.Sum(i => i.Quantity * i.UnitPrice),
    //          })
    //          .ToListAsync();
    //        
    //      //Ranking klientów wg łącznej wartości zamówień (GroupBy + Sum + OrderByDescending).
    //      var customerStats = await db.Orders
    //          .GroupBy(o => o.Customer.FullName)
    //          .Select(orderGroup => new
    //          {
    //              FullName = orderGroup.Key, 
    //              OrderStats = orderGroup.Sum(o => o.Items.Sum(i => i.Quantity * i.UnitPrice))
    //          })
    //          .OrderByDescending(o => o.OrderStats)
    //          .ToListAsync();
    //        
    //      //Średnia wartość zamówienia per miasto klienta (join przez nawigację).
    //      var cityAvg = await db.Orders
    //          .GroupBy(o => o.Customer.City)
    //          .Select(cityGroup => new
    //          {
    //              City = cityGroup.Key,
    //              CityAvg = cityGroup.Average(o => o.Items.Sum(i => i.Quantity * i.UnitPrice))
    //          })
    //          .ToListAsync();
    //        
    //      //Produkty, które nigdy nie zostały zamówione (anti-join: Where(p => !p.OrderItems.Any())).
    //      var neverOrdered = await db.Products
    //          .Where(p => !p.OrderedItems.Any())
    //          .Select(p => p.Name)
    //          .ToListAsync();
    //        
    //      //Dynamicznie budowane zapytanie — użytkownik podaje opcjonalny filtr statusu i minimalną kwotę, zapytanie buduje się warunkowo (IQueryable + if).
    //      System.Console.WriteLine("Please select a number from 1 to 5 to pick your status filter. Each number corresponds to a different filter:\n" +
    //                               "1 - New\n" +
    //                               "2 - Validated\n" +
    //                               "3 - Processing\n" +
    //                               "4 - Completed\n" +
    //                               "5 - Cancelled\n" +
    //                               "Press enter or any other character to skip");
    //      Order.Status? filterStatus;
    //      switch (System.Console.ReadKey().KeyChar)
    //      {
    //          case '1':
    //              filterStatus = Order.Status.New;
    //              break;
    //          case '2':
    //              filterStatus = Order.Status.Validated;
    //              break;
    //          case '3':
    //              filterStatus = Order.Status.Processing;
    //              break;
    //          case '4':
    //              filterStatus = Order.Status.Completed;
    //              break;
    //          case '5':
    //              filterStatus = Order.Status.Cancelled;
    //              break;
    //          default:
    //              filterStatus = null;
    //              break;
    //      }
    //        
    //      System.Console.WriteLine("Please enter a minimum value for filtering orders. Press Enter to skip.");
    //      string? userInput = System.Console.ReadLine();
    //      decimal minPrice = 0m;
    //        
    //      if (!string.IsNullOrEmpty(userInput) && decimal.TryParse(userInput, out decimal parsedPrice))
    //      {
    //          minPrice = parsedPrice;
    //      }
    //        
    //      IQueryable<Order> orderQuery = db.Orders;
    //      orderQuery = orderQuery.Include(o => o.Customer);
    //      if (filterStatus != null)
    //      {
    //          orderQuery = orderQuery.Where(o => o.CurrentStatus == filterStatus.Value);
    //      }
    //        
    //      if (minPrice > 0)
    //      {
    //          orderQuery = orderQuery.Where(o => o.Items.Sum(i => i.UnitPrice * i.Quantity) > minPrice);
    //      }
    //        
    //      var orderQueryResult = await orderQuery.ToListAsync();
    //        
    //      foreach (var order in vipOrders)
    //      {
    //          System.Console.WriteLine($"Customer: {order.FullName}, Order Id: {order.OrderId}, TotalPrice: {order.TotalPrice}");
    //      }
    //        
    //      System.Console.WriteLine("Ranking of the most spending customers:");
    //      foreach (var order in customerStats)
    //      {
    //         System.Console.WriteLine($"{order.FullName} - {order.OrderStats}$");
    //      }
    //    
    //     System.Console.WriteLine("Average spending per city:");
    //     foreach (var order in cityAvg)
    //     {
    //         System.Console.WriteLine($"{order.City} - {order.CityAvg}$ average spending.");
    //     }
    //    
    //     if (neverOrdered.Count > 0)
    //     {
    //         foreach (var product in neverOrdered)
    //         {
    //             System.Console.WriteLine($"This item {product} has never been ordered.");
    //         }
    //     }
    //     else
    //     {
    //         System.Console.WriteLine("Every item has been ordered at least once.");
    //     }
    //    
    //     System.Console.WriteLine("List of orders within the specified criteria:");
    //     foreach (var order in orderQueryResult)
    //     {
    //         System.Console.WriteLine(order.ToString());
    //     }
    //    
    //     System.Console.WriteLine("-------------------------------------------------" +
    //                              "\nDisplaying successful use of transactions" +
    //                              "\n-------------------------------------------------");
    //     await ProcessOrderAsync(db, 1);
    //     System.Console.WriteLine("-------------------------------------------------" +
    //                              "\nDisplaying unsuccessful use of transactions" +
    //                              "\n-------------------------------------------------");
    //     var customer = await db.Customers.FirstAsync();
    //     var testOrder = new Order(customer);
    //     
    //     var productForFail = await db.Products.FirstAsync();
    //     testOrder.Items.Add(new OrderItem(productForFail, 100, productForFail.Price));
    //     db.Orders.Add(testOrder);
    //     await db.SaveChangesAsync();
    //     try
    //     {
    //         await ProcessOrderAsync(db, testOrder.OrderId);
    //     }
    //     catch (Exception exception)
    //     {
    //         System.Console.WriteLine(exception.Message);
    //     }
    // }
    //     
    // static async Task ProcessOrderAsync(OrderFlowContext db, int orderId)
    // {
    //     await using var transaction = await db.Database.BeginTransactionAsync();
    //     
    //     try
    //     {
    //         var currentOrder = await db.Orders
    //             .Include(o => o.Items)
    //             .ThenInclude(i => i.Product)//Żeby uniknąć ciągłego pytania bazy danych o dopasowanie produktu 
    //             .FirstAsync(o => o.OrderId == orderId);
    //         currentOrder.CurrentStatus =  Order.Status.Processing;
    //         await db.SaveChangesAsync();
    //
    //         foreach (var item in currentOrder.Items)
    //         {
    //             var product = item.Product; //Zastąpiło: var product = await db.Products.FindAsync(item.ProductId);
    //             if (product!.Stock < item.Quantity)
    //             {
    //                 throw new InvalidOperationException($"Product {product.Name} is out of stock.");
    //             }
    //             product.Stock -= item.Quantity;
    //         }
    //         await db.SaveChangesAsync();
    //         
    //         currentOrder.CurrentStatus = Order.Status.Completed;
    //         await db.SaveChangesAsync();
    //         
    //         await transaction.CommitAsync();
    //         
    //     }
    //     catch (Exception)
    //     {
    //         await transaction.RollbackAsync();
    //         throw;
    //     }

        // var order = SampleData.ListOfOrders[0];
        // System.Console.WriteLine($"Order Id: {order.Items[1].Product.Name}");
        // var validator = new OrderValidator(order);
        // validator.CheckIfCartEmpty(out var errorMessage);
    }
}