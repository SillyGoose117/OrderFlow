using OrderFlow.Console.Services;

namespace OrderFlow.Console;
using OrderFlow.Console.Data;
using OrderFlow.Console.Models;
class Program

{
    static void Main(string[] args)
    {
        //Przykład poprawengo zamówienia:
        var goodOrder = SampleData.ListOfOrders[0];
        var myGoodValidator = new OrderValidator(goodOrder);
        myGoodValidator.ValidateAll();
        System.Console.WriteLine("----------------------------");
        
        //Przykład zamówienia łamiącego walidację:
        var badOrder = SampleData.ListOfOrders[1]; 
        var myBadValidator = new OrderValidator(badOrder);
        myBadValidator.ValidateAll();
        System.Console.WriteLine("----------------------------");
        
        //Przykłady działania klasy OrderProcessor:
        var everyOrder = SampleData.ListOfOrders;
        var expensiveOrderAmount = 130;
        //1 - Predicate do filtrowania drogich zamówień: 
        var expensiveOrders = OrderProcessor.FilterOrders(everyOrder, singleOrder => singleOrder.TotalAmount > expensiveOrderAmount);
        
        //2 - Predicate do filtrowania ostatnich zamówień:
        var recentOrders = OrderProcessor.FilterOrders(everyOrder, singleOrder => singleOrder.OrderDate >= DateTime.Now.AddDays(-7));
        
        // 3 - Predicate do filtrowania zamówień po statusie "NEW":
        var newOrders = OrderProcessor.FilterOrders(everyOrder, singleOrder => singleOrder.CurrentStatus == Order.Status.New);
        
        
        //1- Action do wyświetlania zamóień
        System.Console.WriteLine("Filtering by expensive orders: ");
        OrderProcessor.ProcessOrders(expensiveOrders, order => System.Console.WriteLine($"{order.OrderId} - {order.Customer.Address} Total order value: {order.TotalAmount}"));
        System.Console.WriteLine("----------------------------");
        System.Console.WriteLine("Filtering by recent orders: ");
        OrderProcessor.ProcessOrders(recentOrders, order => System.Console.WriteLine($"{order.OrderId} - {order.Customer.Address} -  {order.OrderDate}"));
        System.Console.WriteLine("----------------------------");
        System.Console.WriteLine("Filtering by new orders: ");
        OrderProcessor.ProcessOrders(newOrders, order => System.Console.WriteLine($"{order.OrderId} - {order.Customer.Address} -  {order.CurrentStatus}"));
        
        //2 - Action do zmiany statusu zamówień
        OrderProcessor.ProcessOrders(everyOrder, order => order.CurrentStatus = Order.Status.Cancelled);
        
        //Func z typem anonimowym tworzy listę typu anon
        var orderReport = OrderProcessor.ProjectOrders(everyOrder, order => new
        {
            Id = order.OrderId, 
            FullInfo = $"{order.Customer.Name} {order.Customer.LastName} spent {order.TotalAmount}"
        });
        //Wypisanie wszystkich anonimowych typów jako podsumowanie informacji z zamówień
        System.Console.WriteLine("Order report:");
        foreach (var order in orderReport)
        {
            System.Console.WriteLine($"ID: {order.Id} | Summary: {order.FullInfo}");
        }
        System.Console.WriteLine("----------------------------");
        
        //Agregacja (suma, średnia, max):
        decimal totalSales = OrderProcessor.AggregateOrders(everyOrder, list => list.Sum(order => order.TotalAmount)); //Suma wartości zamówień
        decimal avgSales = OrderProcessor.AggregateOrders(everyOrder, list => list.Average(order => order.TotalAmount)); //Średnia zamówień
        decimal maxSales = OrderProcessor.AggregateOrders(everyOrder, list => list.Max(order => order.TotalAmount)); //Najdroższe zamówienie
        
        System.Console.WriteLine($"Sales report: Sum={totalSales}$ , Avg={avgSales}$, Max={maxSales}$");
        System.Console.WriteLine("----------------------------");
        
        
        //Końcowy łańcuch poleceń (filtruj → sortuj → weź top N → wypisz, używając Predicate, Func i Action):
        OrderProcessor.ProcessOrders(
            OrderProcessor.FilterOrders(everyOrder, order => order.OrderDate <= DateTime.Now.AddDays(-7))
                    .OrderBy(order => order.OrderDate)
                    .Take(2)
                    .ToList(),
                order => System.Console.WriteLine("Selected order date: " + order.OrderDate)
                );
    }
}