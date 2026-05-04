using OrderFlow.Console.Persistence;
using OrderFlow.Console.Services;

namespace OrderFlow.Console;
using Data;
using Models;

class Program

{
    static async Task Main(string[] args)
    {
        var everyOrder = SampleData.ListOfOrders;
        var repo = new OrderRepository();
        
        await repo.SaveToJsonAsync(everyOrder, "TestFiles/orders");
        await repo.SaveToXmlAsync(everyOrder, "TestFiles/orders.xml");
        
        System.Console.WriteLine("--Fecthing saved files--");
        var loadedFromJson = await repo.LoadFromJsonAsync("TestFiles/orders.json");
        var loadedFromXml = await repo.LoadFromXmlAsync("TestFiles/orders.xml");
        
        System.Console.WriteLine($"Amount of orders locally - {everyOrder.Count}");
        System.Console.WriteLine($"Amount of Json orders - {loadedFromJson.Count}");
        System.Console.WriteLine($"Amount of orders locally - {loadedFromXml.Count}");
        
        System.Console.WriteLine("--Price comparison--");
        var localSum = everyOrder.Sum(o => o.TotalAmount);
        var jsonSum = loadedFromJson.Sum(o => o.TotalAmount);
        var xmlSum = loadedFromXml.Sum(o => o.TotalAmount);
        
        System.Console.WriteLine($"Total amount from orders stored locally - {localSum}");
        System.Console.WriteLine($"Total amount from Json orders - {jsonSum}");
        System.Console.WriteLine($"Total amount from xml orders - {xmlSum}");
    }
}