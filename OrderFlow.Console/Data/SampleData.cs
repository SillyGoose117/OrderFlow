using OrderFlow.Console.Models;
namespace OrderFlow.Console.Data;

public class SampleData
{
    public static List<Product> ProductList = new List<Product>
    {
        new Product("Newspaper", 1.5m, 1, "Magazines"),
        new Product("Soy Sauce Packet", 0.9m, 410, "Condiments"),
        new Product("Clay Pot", 3m, 200, "Gardening"),
        new Product("Twister - The game", 12m, 125, "Tabletop Games"),
        new Product("Cat", 10000m, 1, "Creatures 'n' such"),
        new Product("Hair gel", 8.99m, 68, "Beauty Products"),
        new Product("Unnecessarily expensive watch with no particular brand", 3155m, 10, "Accessories - Watches"),
    };

    public static List<Customer> ListOfCustomers = new List<Customer>
    {
        new Customer("John", "Pork", "JohnLikesPork@sillymail.com", "414-505-121", "Warsaw", "ABC st.", true),
        new Customer("Jane", "Park", "JaneEleven@sillymail.com", "564-125-153", "Warsaw", "Somewhere st.", false),
        new Customer("Jack", "Peak", "JackIsAwesome@sillymail.com", "674-756-341", "Hamburg", "Nowhere st.", true),
        new Customer("", "", "JosephPack@sillymail.com", "754-005-111", "", "Over There st.", false),
        new Customer("Jannet", "Poke", "Poke@sillymail.com", "476-235-029", "Hamburg", "Here st.", false),
        new Customer("Jacob", "Lash", "MassiveMuscles@Sillymail.com", "765-676-712", "New York", "Boosh st.", true),
        new Customer("Leon", "Kennedy", "Backflips4Life@sillymail.com", "423-456-657", "Raccoon City", "Bingo st.",  false)
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
            OrderDate = DateTime.Now,
            CurrentStatus = Order.Status.New 
        },
        new Order(ListOfCustomers[1])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[0], 2),
                new OrderItem(ProductList[1], 2),
                new OrderItem(ProductList[2], 2)
            },    
            OrderDate = DateTime.Now.AddDays(5),
            CurrentStatus = Order.Status.Processing 
        },
        new Order(ListOfCustomers[2])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[2], 1), // Clay Pot
                new OrderItem(ProductList[3], 1)  // Twister
            },
            OrderDate = DateTime.Now.AddDays(-6),
            CurrentStatus = Order.Status.Processing 
        },
        new Order(ListOfCustomers[3])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[1], 153) // Soy Sauce
            },
            OrderDate = DateTime.Now.AddDays(-9),
            CurrentStatus = Order.Status.Validated
        },
        new Order(ListOfCustomers[0])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[3], 1) // Twister
            },
            OrderDate = DateTime.Now.AddDays(-10),
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
            OrderDate = DateTime.Now.AddDays(-15),
            CurrentStatus = Order.Status.Cancelled
        },
        new Order(ListOfCustomers[5])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[5], 59), // Hair gel

            },
            OrderDate = DateTime.Now,
            CurrentStatus = Order.Status.New
            },
        new Order(ListOfCustomers[6])
        {
            Items = new List<OrderItem>
            {
                new OrderItem(ProductList[6], 2), // Expensive watch
                new OrderItem(ProductList[1], 112), // Soy Sauce
                new OrderItem(ProductList[5], 1)  // Hair gel
            },
            OrderDate = DateTime.Now,
            CurrentStatus = Order.Status.New
        }
    };
}