using OrderFlow.Console.Persistence;
using OrderFlow.Console.Services;
using Microsoft.EntityFrameworkCore;

namespace OrderFlow.Console;

class Program

{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var currencyService = new CurrencyService(httpClient);
        var converter = new OrderCurrencyConverter(currencyService);
        await using var db = new OrderFlowContext();
        await db.Database.MigrateAsync();
        await DatabaseSeeder.SeedAsync(db);

        var testOrders = db.Orders.Include(o => o.Items).Take(3).ToList();
        
        System.Console.WriteLine("\n--- Conversion rates report ---");

        foreach (var order in testOrders)
        {
            try
            {
                var amountUsd = await converter.ConvertOrderTotalAsync(order, "USD");
                var amountEur = await converter.ConvertOrderTotalAsync(order, "EUR");

                System.Console.WriteLine($"Order #{order.OrderId} | " +
                                         $"PLN: {order.TotalAmount} zł | " +
                                         $"USD: {amountUsd:F2} $ | " +
                                         $"EUR: {amountEur:F2} €"); //Akurat ten znaczek nie działa
            }
            catch (CurrencyServiceException ex)
            {
                System.Console.WriteLine($"Order #{order.OrderId} | Error during conversion: {ex.Message}");
            }
        }
    }
}