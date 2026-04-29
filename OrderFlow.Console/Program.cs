using OrderFlow.Console.Services;

namespace OrderFlow.Console;
using Data;
using Models;

class Program

{
    static void Main(string[] args)
    {
        var everyOrder = SampleData.ListOfOrders;
        var statistics = new OrderStatistics();

        Parallel.ForEach(everyOrder, order =>
        {
            statistics.CollectStats(order);
        });
        statistics.PrintStats();
        System.Console.WriteLine("----------------------------------------------------");
        var everyOrderButBigger = Enumerable.Repeat(everyOrder, 200).SelectMany(x => x).ToList();

        Parallel.ForEach(everyOrderButBigger, order =>
        {
            statistics.CollectStats(order);
            Thread.Sleep(10);
        });
        statistics.PrintStats();
    }
}