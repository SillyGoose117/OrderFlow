using OrderFlow.Console.Services;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OrderFlow.Console;
using Data;
using Models;
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
        
        //Zapytania LINQ:
        System.Console.WriteLine("----------------------------");
        
        //Ten zapis wydaje się bardziej czytelny
        //Szczególnie z uwagi na prostszy syntax "SelectMany" zamiast "from from"
        var itemsUnderTen = everyOrder
            .SelectMany(order => order.Items)
            .Where(item => item.product.Price < 10)
            .Select(item => new
            {
                item.product.Name,
                item.product.Price
            })
            .Distinct();
        
        // var itemsUnderTen2 = from  order in everyOrder
        //     from item in order.Items
        //     where item.product.Price < 10
        //     select new {item.product.Name, item.product.Price};
        
        System.Console.WriteLine("\"Showing list of items that cost less than 10$:");   
        
        foreach (var item in itemsUnderTen)
        {
            System.Console.WriteLine($"{item.Name} - {item.Price}");
        }
        System.Console.WriteLine("----------------------------");
        
        var topClients = everyOrder
            .GroupBy(eliteCustomer => eliteCustomer.Customer.LastName)
            .Select(groupOfClients => new
            {
                ClientName = groupOfClients.Key,
                TotalAmountSpent = groupOfClients.Sum(o => o.TotalAmount)
            })
            .Where(result => result.TotalAmountSpent > 100);
        
        //Niepotrzebnie wprowadza "let" i "where" w dziwnych miejscach
        //W method syntax ciąg jest to po prostu bardziej naturalny 
        // var topClients2 = from o in everyOrder
        //     group order by order.Customer.LastName into orderGroup
        //     let totalAmountSpent = orderGroup.Sum(o => o.TotalAmount)
        //     where  totalAmountSpent > 100
        //     select new { ClientName = orderGroup.Key, TotalAmountSpent = orderGroup.Sum(o => o.TotalAmount) };
        
        System.Console.WriteLine("Showing a list of top clients:");
        
        foreach (var topClient in topClients)
        {
            System.Console.WriteLine($"{topClient.ClientName} - {topClient.TotalAmountSpent}");
        }
        System.Console.WriteLine("----------------------------");
        
        //W porównaniu do "method syntax" tutaj możemy coś zrozumieć bez zaglądania do dokumentacji
        var ordersPerCustomer = from customer in SampleData.ListOfCustomers 
            join order in everyOrder on customer.LastName equals order.Customer.LastName into orders
            select new {FullName = customer.Name + " " + customer.LastName, OrderCount = orders.Count()};
        
        //Ciężko jest się tutaj połapać bez dokumentacji przez skomplikowane osadzenie kluczy
        //Dosyć mało naturalne 
        // var ordersPerCustomer2 = SampleData.ListOfCustomers
        //     .GroupJoin(everyOrder, customer => customer.LastName, order => order.Customer.LastName,
        //         (customer, customerOrders) => new
        //         {
        //             FullName = customer.Name + " " + customer.LastName,
        //             OrderCount = customerOrders.Count()
        //         });
        
        System.Console.WriteLine("Showing a list of customers and how many orders they've placed:");
        
        foreach (var customer in ordersPerCustomer)
        {
            System.Console.WriteLine($"{customer.FullName} - {customer.OrderCount}");
        }
        
        System.Console.WriteLine("----------------------------");
        
        //Mieszanie składni wedle treści zadania
        var favoriteCategory = (from order in everyOrder
                where order.Customer.LastName == "Lash"
                select order)
            .SelectMany(order => order.Items)
            .GroupBy(item => item.product.Category)
            .OrderByDescending(categoryGroup => categoryGroup.Count())
            .FirstOrDefault()?.Key;
        
        // var favoriteCategory1 = everyOrder
        //     .Where(order => order.Customer.LastName == "Lash")
        //     .SelectMany(order => order.Items)
        //     .GroupBy(item => item.product.Category)
        //     .OrderByDescending(categoryGroup => categoryGroup.Count())
        //     .Select(group => group.Key)
        //     .FirstOrDefault();
        
        //Nie posiada odpowiednika "FirstOrDefault"?
        // var favoriteCategory2 = from order in everyOrder
        //     from item in order.Items
        //     where order.Customer.LastName == "Lash"
        //     group item by item.product.Category
        //     into categoryGroup2
        //     orderby categoryGroup2.Count() descending
        //     select new {CategoryName = categoryGroup2.Key};
        
        System.Console.WriteLine($"Favorite product category of Lash: {favoriteCategory}");
        
        System.Console.WriteLine("----------------------------");
        
        //Moim zdaniem to drobne "into" w "group by" ziększa czytelność tego zapytania
        var ordersPerCity = from order in everyOrder
            group order by order.Customer.City into cityGroup
            select new {CityName = cityGroup.Key, CityCount = cityGroup.Count()};
        
        // var orderPerCity2 = everyOrder
        //     .GroupBy(order => order.Customer.City)
        //     .Select(groupOfCities => new
        //     {
        //         City = groupOfCities.Key,
        //         CityCount = groupOfCities.Count()
        //     });

        System.Console.WriteLine("Orders by different cities of customers:");
        foreach (var cities in ordersPerCity) System.Console.WriteLine($"{cities.CityName}: {cities.CityCount}");
        
        System.Console.WriteLine("----------------------------");
        
        
        var categoryAvgProfits = everyOrder
            .SelectMany(item => item.Items)
            .GroupBy(item => item.product.Category)
            .Select(order => new
            {
                CategoryName = order.Key,
                AvgProfit = order.Average(item => item.TotalPrice)
            })
            .OrderByDescending(item => item.AvgProfit);
            
        //Znowu niepotrzebne "let" na którym musimy wykonać operację "average"
        // var categoryAvgProfits2 = from order in everyOrder
        //     from item in order.Items
        //     group item by item.product.Category into profitGroup
        //     let averageProfit = profitGroup.Average(item => item.TotalPrice)
        //     orderby profitGroup descending 
        //     select new {CategoryName2 = profitGroup.Key, Profit = profitGroup.Average(item => item.TotalPrice)};
        
        foreach (var categoriesAvg in categoryAvgProfits)
        {
            System.Console.WriteLine($"Displaying average profits from all the categories: {categoriesAvg.CategoryName} - {categoriesAvg.AvgProfit}");
        }
    }
}