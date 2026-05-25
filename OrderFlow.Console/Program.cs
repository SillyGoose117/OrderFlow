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
        var everyOrder = SampleData.ListOfOrders;
        
        System.Console.WriteLine("==========================");
        System.Console.WriteLine("Zadanie 1");
        System.Console.WriteLine("==========================");
        
        var repo1 = new OrderRepository();
        
        await repo1.SaveToJsonAsync(everyOrder, "TestFiles/orders.json");
        await repo1.SaveToXmlAsync(everyOrder, "TestFiles/orders.xml");
        
        System.Console.WriteLine("--Fetching saved files--");
        var loadedFromJson = await repo1.LoadFromJsonAsync("TestFiles/orders.json");
        var loadedFromXml = await repo1.LoadFromXmlAsync("TestFiles/orders.xml");
        
        System.Console.WriteLine($"Amount of orders locally - {everyOrder.Count}");
        System.Console.WriteLine($"Amount of Json orders - {loadedFromJson.Count}");
        System.Console.WriteLine($"Amount of Xml orders  - {loadedFromXml.Count}");
        
        System.Console.WriteLine("--Price comparison--");
        var localSum = everyOrder.Sum(o => o.TotalAmount);
        var jsonSum = loadedFromJson.Sum(o => o.TotalAmount);
        var xmlSum = loadedFromXml.Sum(o => o.TotalAmount);
        
        System.Console.WriteLine($"Total amount from orders stored locally - {localSum}");
        System.Console.WriteLine($"Total amount from Json orders - {jsonSum}");
        System.Console.WriteLine($"Total amount from Xml orders - {xmlSum}");
        
        System.Console.WriteLine("==========================");
        System.Console.WriteLine("Zadanie 2");
        System.Console.WriteLine("==========================");
        
        var makeAReport = new XmlReportBuilder();
        var threshold = 1000m;
        var aReport = makeAReport.BuildReport(everyOrder);
        await makeAReport.SaveReportAsync(aReport, "TestFiles/report.xml");
        var showExpensiveOrders = await makeAReport.FindingHighValueOrderIdsAsync("TestFiles/report.xml", threshold);
        System.Console.WriteLine($"Showing expensive orders valued at more than {threshold}$");
        foreach (var expensiveOrder in showExpensiveOrders)
        {
            System.Console.WriteLine($"Order ID - {expensiveOrder}");
        }
        
        System.Console.WriteLine("==========================");
        System.Console.WriteLine("Zadanie 3");
        System.Console.WriteLine("==========================");
        
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");
        var pipeline = new OrderPipeline();
        using var watcher = new InboxWatcher(path, pipeline);
        var repo3 = new OrderRepository();
        var orders = SampleData.ListOfOrders;
        
        for (var i = 0; i <= 3; i++)
        {
            await repo3.SaveToJsonAsync(orders, Path.Combine(path, $"File{i}.json"));
            await Task.Delay(2000);
        }
        System.Console.ReadKey();
    }
}