using System.Collections.Concurrent;

namespace OrderFlow.Console.Services;
using OrderFlow.Console.Models;

public class OrderStatistics
{
    private int totalProcessed = 0;
    private decimal totalRevenue = 0;
    private ConcurrentDictionary<Order.Status, int> ordersPerStatus = new ConcurrentDictionary<Order.Status, int>();
    private List<string> processingErrors = new List<string>();
    //    private ordersPerStatus = new Dictionary<Order.Status, int>();
    //    private processingErrors = new List<string>();
    
    //Metoda z błędami, która nie posiada zabezpieczeń
    // public void CollectStats(Order order)
    // {
    //     totalProcessed++;
    //     totalRevenue += order.TotalAmount;
    //     if (!ordersPerStatus.TryAdd(order.CurrentStatus, 1))
    //     {
    //             ordersPerStatus[order.CurrentStatus]++;
    //     } else
    //     {
    //         ordersPerStatus.Add(order.CurrentStatus, 1);
    //     }
    //     processingErrors.Add("This order has encountered a validation error.");   
    //
    //     System.Console.WriteLine($"Displaying stats for processed orders:\n" +
    //                              $"Total orders processed: {totalProcessed}\n" +
    //                              $"Total revenue obtained: {totalRevenue}\n" +
    //                              $"Different order statuses: {ordersPerStatus.Count}\n" +
    //                              $"Number of errors: {processingErrors.Count},");
    // }
    
    
    //Metoda z zabezpieczeniami
    private System.Threading.Lock locked = new Lock();
    public void CollectStats(Order order)
    {
        ordersPerStatus.AddOrUpdate(order.CurrentStatus, 1, (key, existingValue) => existingValue + 1);
        Interlocked.Increment(ref totalProcessed);   
        lock (locked)
        {
            totalRevenue += order.TotalAmount;
            if (order.CurrentStatus == Order.Status.Cancelled)
            {
                processingErrors.Add("This order has encountered a validation error.");     
            }
        }
    }

    public void PrintStats()
    {
        System.Console.WriteLine($"Displaying stats for processed orders:\n" + 
                                 $"Total orders processed: {totalProcessed}\n" + 
                                 $"Total revenue obtained: {totalRevenue}\n" + 
                                 $"Different order statuses: {ordersPerStatus.Count}\n" + 
                                 $"Number of errors: {processingErrors.Count}.");
    }
}
