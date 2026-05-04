using System.Text.Json;
using System.Text.Json.Serialization;
using OrderFlow.Console.Models;
using System.Xml.Serialization;
using OrderFlow.Console.Services;

namespace OrderFlow.Console.Persistence;

public class OrderRepository
{
    
    public async Task SaveToJsonAsync(IEnumerable<Order> orders, string path)
    {
        var goodJsonFormat = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
        var jsonDirectory = Path.GetDirectoryName(path);
        if (Directory.Exists(path))
        {
            System.Console.WriteLine("Catalogue already exists!");
        }
        else
        {
            Directory.CreateDirectory(jsonDirectory);
        }

        await using (var writeJsonStream = File.Create(path))
        {
            await JsonSerializer.SerializeAsync(writeJsonStream, orders, goodJsonFormat);   
        }
    }

    public async Task<List<Order>> LoadFromJsonAsync(string path)
    {
        if (!File.Exists(path))
        {
            System.Console.WriteLine("Json file not found! Returning an empty list as an exception.");
            return new List<Order>();
        }

        var jsonReadOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        
        await using (var openJsonStream = File.OpenRead(path))
        {
            var jsonOrders = await JsonSerializer.DeserializeAsync<List<Order>>(openJsonStream, jsonReadOptions);
            return jsonOrders ?? new List<Order>();
        }
    }
    
    public async Task SaveToXmlAsync(IEnumerable<Order> orders, string path)
    {
        var xmlDirectory = Path.GetDirectoryName(path);
        if (Directory.Exists(path))
        {
            System.Console.WriteLine("Catalogue already exists!");
        }
        else
        {
            Directory.CreateDirectory(xmlDirectory);
        }

        var xmlSerializer = new XmlSerializer(typeof(List<Order>));
        
        await using (var writeXmlStream = File.Create(path))
        {
            xmlSerializer.Serialize(writeXmlStream, orders);
        }
    }

    public async Task<List<Order>> LoadFromXmlAsync(string path)
    {
        if (!File.Exists(path))
        {
            System.Console.WriteLine("XML file not found! Returning an empty list as an exception.");
            return new List<Order>();
        }
        
        var xmlSerializer = new XmlSerializer(typeof(List<Order>));

        await using (var openXmlStream = File.OpenRead(path))
        {
            var xmlOrders = (List<Order>?) xmlSerializer.Deserialize(openXmlStream);
            return xmlOrders ?? new List<Order>();
        }
    }
}