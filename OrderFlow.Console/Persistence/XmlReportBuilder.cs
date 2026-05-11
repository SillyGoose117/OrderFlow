using System.Xml.Linq;
using OrderFlow.Console.Models;
using System.Xml.Serialization;

namespace OrderFlow.Console.Persistence;

public class XmlReportBuilder
{
    //Stwórz plik / drzewko Xml
    public XDocument BuildReport(IEnumerable<Order> orders)
    { 
        var statusElements = orders
            .GroupBy(o => o.CurrentStatus)
            .Select(statusGroup => new XElement("status", new XAttribute("name", statusGroup.Key),
                new XAttribute("count", statusGroup.Count()),
                new XAttribute("revenue", statusGroup.Sum(o => o.TotalAmount))));

        var customerElements = orders
            .GroupBy(o => o.Customer)
            .Select(customerGroup => new XElement("customer",
                new XAttribute("id", customerGroup.Key.CustomerId),
                new XAttribute("name", customerGroup.Key.FullName),
                new XAttribute("isVip", customerGroup.Key.IsVIP),
                    new XElement("orderCount", customerGroup.Count()),
                    new XElement("totalSpent", customerGroup.Sum(o => o.TotalAmount)),
                    new XElement("orders",
                        customerGroup.Select(o => 
                            new XElement("orderRef", 
                                new XAttribute("id", o.OrderId),
                                new XAttribute("total", o.TotalAmount))))));
            
        
        var report = new XDocument(
            new XElement("report", new XAttribute("generated", DateTime.Now.ToString("s")), 
                new XElement("summary", new XAttribute("totalOrders", orders.Count()), new XAttribute("totalRevenue", orders.Sum(o => o.TotalAmount))),
                new XElement("byStatus", statusElements),
                new XElement("byCustomer", customerElements)
            ));

        return report;
    }

    //Zapisz plik / drzewko Xml
    public async Task SaveReportAsync(XDocument report, string path)
    {
        var xmlDirectory = Path.GetDirectoryName(path);
        if (Directory.Exists(xmlDirectory))
        {
            System.Console.WriteLine("Catalogue already exists!");
        }
        else
        {
            Directory.CreateDirectory(xmlDirectory);
        }
        
        await using (var writeXmlStream = File.Create(path))
        {
          await report.SaveAsync(writeXmlStream,  SaveOptions.None, CancellationToken.None);
        }
        System.Console.WriteLine($"Report saved to {path}!");
    }

    // Zmieniłem typ zwracany na Guid zamiast int, mam nadzieje, że to nie problem w kwestii zadania
    public async Task<IEnumerable<Guid>> FindingHighValueOrderIdsAsync(string reportPath, decimal threshold)
    {
        await using var readStream = File.OpenRead(reportPath);
        XDocument loadReport = await XDocument.LoadAsync(readStream, LoadOptions.None, CancellationToken.None);
        IEnumerable<XElement> ordersFromReport = loadReport.Descendants("orders");
        
        var expensiveOrders = ordersFromReport.Descendants("orderRef")
            .Where(orderElement => (decimal)orderElement.Attribute("total") > threshold)
            .Select(orderElement => (Guid)orderElement.Attribute("id"));
        return expensiveOrders;
    }
}