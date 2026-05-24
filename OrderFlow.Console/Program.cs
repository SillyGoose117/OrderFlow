using OrderFlow.Console.Services;

namespace OrderFlow.Console;
using Data;
using Models;

class Program

{
    static async Task Main(string[] args)
    {
        var everyOrder = SampleData.ListOfOrders;
        var order1 = SampleData.ListOfOrders[0];
        var order2 = SampleData.ListOfOrders[1];
        var order3 = SampleData.ListOfOrders[2];
        var order4 = SampleData.ListOfOrders[3];
        var order5 = SampleData.ListOfOrders[4];
        var order6 = SampleData.ListOfOrders[5];
        
        System.Console.WriteLine("--------------------------------------------------" +
                                 "\nZadanie 1" +
                                 "\n--------------------------------------------------");
        //Symulacja zamówień z reakcją subskrybentów:
        var orderPipeline = new OrderPipeline();
        var accounting = new Accounting();
        var log =  new Log();
        var warehouse = new Warehouse();
        
        orderPipeline.StatusChanged += warehouse.OnStatusChanged;
        orderPipeline.StatusChanged += accounting.GoodAccount;
        orderPipeline.StatusChanged += log.LogThisGoodOrder;
        orderPipeline.ValidationCompleted += log.LogThisBadOrder;
        orderPipeline.ProcessOrder(order1);
        orderPipeline.ProcessOrder(order4);
        
        System.Console.WriteLine("--------------------------------------------------" +
                                 "\nZadanie 2" +
                                 "\n--------------------------------------------------");
        //ProcessOrderAsync
        var externalServiceSimulator = new ExternalServiceSimulator();
        await externalServiceSimulator.ProcessOrderAsync(order1);
        
        //ProcessMultipleOrdersAsync
        await externalServiceSimulator.ProcessMultipleOrdersAsync(everyOrder);
        
        System.Console.WriteLine("--------------------------------------------------" +
                                 "\nZadanie 3" +
                                 "\n--------------------------------------------------");
        
        var statistics = new OrderStatistics();
        
        Parallel.ForEach(everyOrder, order =>
        {
            statistics.CollectStats(order);
        });
        statistics.PrintStats();
        System.Console.WriteLine("----------------------------------------------------");
        var everyOrderButBigger = Enumerable.Repeat(everyOrder, 200).SelectMany(x => x).ToList();
        var statistics2 =  new OrderStatistics();
        
        Parallel.ForEach(everyOrderButBigger, order =>
        {
            statistics2.CollectStats(order);
            Thread.Sleep(10);
        });
        statistics2.PrintStats();
    }
}