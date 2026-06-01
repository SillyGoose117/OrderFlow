using Moq;
using OrderFlow.Console.Models;
using OrderFlow.Console.Services;

namespace OrderFlow.Tests;

public class CurrencyConverterTests
{
    [Fact]
    public async Task Check_CorrectPath_ReturnsConversion()
    {
        //Arrange
        var order = new Order { };
        var orderItemTest = new OrderItem { Quantity = 2 , UnitPrice = 50 };
        order.Items.Add(orderItemTest);
        var targetCurrency = "USD";
        var mockConverter = new Mock<ICurrencyService>();
        mockConverter
            .Setup(converter => converter.ConvertAsync(order.TotalAmount, "PLN", "USD"))
            .ReturnsAsync(25.0m);
        var orderCurrencyConverter = new OrderCurrencyConverter(mockConverter.Object);
        //Act
        var result = await orderCurrencyConverter.ConvertOrderTotalAsync(order, targetCurrency);
        //Assert
        Assert.Equal(25.0m, result);
    }

    [Fact]
    public async Task Check_ErrorHandling_ReturnsError()
    {
        //Arrange
        var order = new Order { };
        var orderItemTest = new OrderItem { Quantity = 2 , UnitPrice = 50 };
        order.Items.Add(orderItemTest);
        var targetCurrency = "USD";
        var mockConverter = new Mock<ICurrencyService>();
        mockConverter
            .Setup(converter => converter.ConvertAsync(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new CurrencyServiceException("A conversion error has occurred."));
        var orderCurrencyConverter = new OrderCurrencyConverter(mockConverter.Object);
        //Act & Assert
        await Assert.ThrowsAsync<CurrencyServiceException>(() => orderCurrencyConverter.ConvertOrderTotalAsync(order, targetCurrency));
    }
}