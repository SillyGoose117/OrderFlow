using OrderFlow.Console.Models;
using OrderFlow.Console.Services;

namespace OrderFlow.Tests;

public class DiscountCalcTests
{
    [Fact]
    public void Calculate_StandardCustomer_NoDiscount()
    {
        //Arrange
        var order = new Order {Customer = new Customer {IsVIP = false}};
        var orderItemTest = new OrderItem { Quantity = 2 , UnitPrice = 50 };
        order.Items.Add(orderItemTest);
        var calculator = new DiscountCalculator();
        //Act
        decimal discount = calculator.CalculateDiscount(order);
        //Assert
        Assert.Equal(0m, discount);
    }

    [Fact]
    public void Calculate_VIPCustomer_Discount()
    {
        //Arrange
        var order = new Order {Customer = new Customer {IsVIP = true}};
        var orderItemTest = new OrderItem { Quantity = 2 , UnitPrice = 50 };
        order.Items.Add(orderItemTest);
        var calculator = new DiscountCalculator();
        //Act
        decimal discount = calculator.CalculateDiscount(order);
        //Assert
        Assert.Equal(10m, discount);
    }

    [Fact]
    public void Calculate_HighValue_Discount()
    {
        //Arrange
        var order = new Order {Customer = new Customer {IsVIP = false}};
        var orderItemTest = new OrderItem { Quantity = 2 , UnitPrice = 1000 };
        order.Items.Add(orderItemTest);
        var calculator = new DiscountCalculator();
        //Act
        decimal discount = calculator.CalculateDiscount(order);
        //Assert
        Assert.Equal(100m, discount);
    }
}