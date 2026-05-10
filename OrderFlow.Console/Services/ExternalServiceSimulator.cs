using System.Diagnostics;
using OrderFlow.Console.Models;
namespace OrderFlow.Console.Services;

public class ExternalServiceSimulator
{
    Random random = new Random();
    //Tymczasowo wyczyściłem zwracane wiadomości, żeby łatwiej można zauważyć watcher'a
    private async Task<string> CheckInventoryAsync(Product product)
    {
        //System.Console.WriteLine($"Beginning inventory inspection of {product.Name}...");
        int delayMs = random.Next(500, 1500);
        await Task.Delay(delayMs);
        
        //return $"{product.Name} Inventory inspection completed.";
        return $"Product {product.Name} done";
    }

    private async Task<string> ValidatePaymentAsync(Order order)
    {
        int delayMs = random.Next(1000, 2000);
        //System.Console.WriteLine($"Beginning payment validation of order {order.OrderId}.");
        await Task.Delay(delayMs);
        //return $"Success! Payment for order {order.OrderId} has been validated.";
        return "Payment Successful";
    }
    
    private async Task<string> CalculateShippingAsync(Order order)
    {
        int delayMs = random.Next(300, 800);
        int shippingPirce = random.Next(10, 45);
        //System.Console.WriteLine($"Calculating shipping details for order {order.OrderId}.");
        await Task.Delay(delayMs);
        //return $"Shipping details have been added to your final order {order.TotalAmount +  shippingPirce}.";
        return "Shipping calculated";
    }

    
    
    public async Task ProcessOrderAsync(Order order)
    {
        System.Console.WriteLine("Simulating...");
        List<Task<string>> combinedTasks = new List<Task<string>>();
        var sw = Stopwatch.StartNew();
        foreach (var item in order.Items)
        {
            var a = CheckInventoryAsync(item.product);
            combinedTasks.Add(a);
        }
        var b = ValidatePaymentAsync(order);
        var c  = CalculateShippingAsync(order);
        combinedTasks.Add(b);
        combinedTasks.Add(c);
        string[] completedSimulatedTasks = await Task.WhenAll(combinedTasks);
        sw.Stop();
        foreach (var task in completedSimulatedTasks)
        {
            System.Console.WriteLine(task);
        }
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms");
    }
    
    public async Task ProcessMultipleOrdersAsync(List<Order> orders)
    {
        List<Task> taskList = new List<Task>();
        var semaphore = new SemaphoreSlim(3, 3); //Limit przetwarzanych zamówień do makssymalnie 3
        var tasksCompleted = 0;
        var taskCounterLock = new object(); //Kłódka dla licznika wykonanych zadań.
        System.Console.WriteLine("=================================================");
        System.Console.WriteLine(" TEST 1: PRZETWARZANIE RÓWNOLEGŁE (MAX 3 NARAZ)");
        System.Console.WriteLine("=================================================");
        var sw = Stopwatch.StartNew();
        
        async Task ProcessOrderUsingSemaphoreAsync(Order semaOrder, SemaphoreSlim semaphore2) //Metoda pomocnicza zapobiega zakleszczeniu, wykonuje zamówienia równolegle
            { 
                await semaphore2.WaitAsync();
                try
                {
                    await ProcessOrderAsync(semaOrder);
                    lock (taskCounterLock)
                    {
                        tasksCompleted++;
                        System.Console.WriteLine($"Processing order, {tasksCompleted} of {orders.Count} orders completed.");
                    }
                }
                finally
                {
                    semaphore2.Release();
                }
            }
        foreach (var order in orders)
        {
            var x = ProcessOrderUsingSemaphoreAsync(order, semaphore);
            taskList.Add(x);
        }
        await Task.WhenAll(taskList);
        sw.Stop();
        System.Console.WriteLine($"\nParallel processing completed in {sw.ElapsedMilliseconds} ms.");
        
        System.Console.WriteLine("\n=================================================");
        System.Console.WriteLine(" TEST 2: PRZETWARZANIE SEKWENCYJNE (PO KOLEI)");
        System.Console.WriteLine("=================================================\n");
        sw.Restart();
        foreach (var order in orders) //Wykonywanie zamówień sekwencyjnioe
        {
            await ProcessOrderAsync(order);
        }
        sw.Stop();
        System.Console.WriteLine($"\nSequential processing completed in {sw.ElapsedMilliseconds} ms.");
    }
}