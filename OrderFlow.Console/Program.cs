using OrderFlow.Console.Persistence;
using OrderFlow.Console.Services;

namespace OrderFlow.Console;
using Data;
using System.Xml.Linq;
using Models;

class Program

{
    static async Task Main(string[] args)
    {
        var everyOrder = SampleData.ListOfOrders;
        var makeAReport = new XmlReportBuilder();
        var aReport = makeAReport.BuildReport(everyOrder);
        await makeAReport.SaveReportAsync(aReport, "TestFiles/report.xml");
        var showExpensiveOrders = await makeAReport.FindingHighValueOrderIdsAsync("TestFiles/report.xml", 1000m);
        foreach (var expensiveOrder in showExpensiveOrders)
        {
            System.Console.WriteLine(expensiveOrder);
        }
    }
}