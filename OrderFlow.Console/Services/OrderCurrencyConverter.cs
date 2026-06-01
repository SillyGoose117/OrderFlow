using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public class OrderCurrencyConverter
{
    private readonly ICurrencyService _currencyService;
    private const string BaseCurrency = "PLN";
    
    public OrderCurrencyConverter (ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public async Task<decimal> ConvertOrderTotalAsync(Order order, string targetCurrency)
    {
        var converted = await _currencyService.ConvertAsync(order.TotalAmount, BaseCurrency, targetCurrency);
        return converted;
    }
}