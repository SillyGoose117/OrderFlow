using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public static class OrderProcessor
{
    
    public static List<Order> FilterOrders(List<Order> orders, Predicate<Order> predicate)
    {
        List<Order> filteredOrders = new List<Order>();
        foreach (var singleOrder in orders)
        {
            if (predicate(singleOrder))
            {
                filteredOrders.Add(singleOrder);
            }
        }
        return filteredOrders;
    }

    public static void ProcessOrders(List<Order> orders, Action<Order> action)
    {
        foreach (var singleOrder in orders)
        {
            action(singleOrder);
        }
    }

    public static List<T> ProjectOrders<T>(List<Order> orders, Func<Order, T> projection)
    {
        List<T> resultList = new List<T>();
        foreach (var singleOrder in orders) 
        {
            resultList.Add(projection(singleOrder));
        }
        return resultList;
    }

    public static decimal AggregateOrders(List<Order> orders, Func<IEnumerable<Order>, decimal> aggregator)
    {
     return aggregator(orders);   
    }
}