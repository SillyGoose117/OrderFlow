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
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");
        var pipeline = new OrderPipeline();
        using var watcher = new InboxWatcher(path, pipeline);
        var repo = new OrderRepository();
        var orders = SampleData.ListOfOrders;
        
        for (var i = 0; i <= 3; i++)
        {
            await repo.SaveToJsonAsync(orders, Path.Combine(path, $"File{i}.json"));
            await Task.Delay(2000);
        }
        
        System.Console.ReadKey();
    }
}
