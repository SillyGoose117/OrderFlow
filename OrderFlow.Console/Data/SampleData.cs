using OrderFlow.Console.Models;
namespace OrderFlow.Console.Data;

public class SampleData
{
    public static List<Product> ProductList = new List<Product>
    {
        new Product("Newspaper", 1.5m, 1, "Daily Use"),
        new Product("Soy Sauce Packet", 0.3m, 410, "Condiments"),
        new Product("Clay Pot", 3m, 200, "Gardening"),
        new Product("Twister - The game", 12m, 125, "Tabletop Games"),
        new Product("Cat", 10000m, 1, "Creatures 'n' such")
    };

    public static List<Customer> ListOfCustomers = new List<Customer>
    {
        new Customer("John", "Pork", "JohnLikesPork@sillymail.com", "414-505-121", "ABC st.", true),
        new Customer("", "Park", "JaneEleven@sillymail.com", "564-125-153", "", false),
        new Customer("Jack", "Peak", "JackIsAwesome@sillymail.com", "674-756-341", "Nowhere st.", false),
        new Customer("Joseph", "Pack", "JosephPack@sillymail.com", "754-005-111", "Over There st.", false),
        new Customer("Jannet", "Poke", "Poke@sillymail.com", "476-235-029", "Here st.", false)
    };

    public static List<Order> ListOfOrders = new List<Order>
    {
        new Order(ListOfCustomers[0])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[0], 1),
                new OrderItem(ProductList[1], 13),
                new OrderItem(ProductList[4], 1)
            },
            CurrentStatus = Order.Status.Processing 
        },
        new Order(ListOfCustomers[1])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[0], 2),
                new OrderItem(ProductList[1], 2),
                new OrderItem(ProductList[2], 2)
            },    
            CurrentStatus = Order.Status.Processing 
        },
        new Order(ListOfCustomers[2])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[2], 1), // Clay Pot
                new OrderItem(ProductList[3], 1)  // Twister
            },
            CurrentStatus = Order.Status.Processing 
        },
        new Order(ListOfCustomers[3])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[1], 5) // Soy Sauce
            },
            CurrentStatus = Order.Status.Validated
        },
        new Order(ListOfCustomers[0])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[4], 1) // Cat
            },
            CurrentStatus = Order.Status.Completed
        },
        new Order(ListOfCustomers[4])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[0], 1), // Newspaper
                new OrderItem(ProductList[1], 2), // Soy Sauce
                new OrderItem(ProductList[2], 1)  // Clay Pot
            },
            CurrentStatus = Order.Status.Cancelled
        }
    };
}