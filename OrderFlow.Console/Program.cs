using OrderFlow.Console.Services;

namespace OrderFlow.Console;
using Data;
using Models;
class Program

{
    static void Main(string[] args)
    {
        //Symulacja zamówień z reakcją subskrybentów:
        var order1 = SampleData.ListOfOrders[0];
        var order2 = SampleData.ListOfOrders[1];
        var order3 = SampleData.ListOfOrders[2];
        var order4 = SampleData.ListOfOrders[3];
        var order5 = SampleData.ListOfOrders[4];
        var order6 = SampleData.ListOfOrders[5];
        var orderPipeline = new OrderPipeline();
        var accounting = new Accounting();
        var log =  new Log();
        var warehouse = new Warehouse();
        
        orderPipeline.StatusChanged += warehouse.OnStatusChanged;
        orderPipeline.StatusChanged += accounting.GoodAccount;
        //orderPipeline.ValidationCompleted += accounting.BadAccount;

        orderPipeline.StatusChanged += log.LogThisGoodOrder;
        orderPipeline.ValidationCompleted += log.LogThisBadOrder;
        
        orderPipeline.ProcessOrder(order1);
        orderPipeline.ProcessOrder(order4);
    }
}