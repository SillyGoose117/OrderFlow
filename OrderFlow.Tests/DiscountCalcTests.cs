using OrderFlow.Console.Models;

namespace OrderFlow.Tests;

public class DiscountCalcTests
{
    [Fact]
    public void Calculate_StandardCustomer_NoDiscount()
    {
        //Arrange
        var order = new Order {Customer = new Customer {IsVIP = false}};
        var testProduct = new Product { Stock = 50};
        var orderItemTest = new OrderItem { Product = testProduct, Quantity = 2 , UnitPrice = 50};
        order.Items.Add(orderItemTest);
        var calculator = new DiscountCalculator();
        //Act
        decimal discount = calculator.CalculateDiscount(order);
        //Assert
        Assert.Equal(0m, discount);
    }
}