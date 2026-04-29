using OrderFlow.Console.Services;

namespace OrderFlow.Console;
using Data;
using Models;
class Program

{
    static async Task Main(string[] args)
    {
        //Symulacja zamówień z reakcją subskrybentów:
        var order1 = SampleData.ListOfOrders[0];
        var order2 = SampleData.ListOfOrders[1];
        var order3 = SampleData.ListOfOrders[2];
        var order4 = SampleData.ListOfOrders[3];
        var order5 = SampleData.ListOfOrders[4];
        var order6 = SampleData.ListOfOrders[5];
        System.Console.WriteLine("-----------------------------------");
        //ProcessOrderAsync
        var externalServiceSimulator = new ExternalServiceSimulator();
        await externalServiceSimulator.ProcessOrderAsync(order1);
        
        //ProcessMultipleOrdersAsync
        var everyOrder = SampleData.ListOfOrders;
        await externalServiceSimulator.ProcessMultipleOrdersAsync(everyOrder);
    }
}