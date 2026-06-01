using System.Net;
using System.Net.Http.Json;
using OrderFlow.Console.Services;

namespace OrderFlow.Tests;

public class TestHttpMessageHandler(Func<HttpResponseMessage> responseFactory) : HttpMessageHandler
{
    public int CallCount { get; private set; } = 0; // Do sprwadzenia specjalnegu przypadku zPLN
    public Uri? CorrectUri { get; private set; }
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        CallCount++;
        CorrectUri = request.RequestUri;
        return Task.FromResult(responseFactory());
    }
}

public class ConversionRatesTest
{
    [Fact]
    public async Task Check_CorrectPath_ReturnsConversion() // (amount * fromCurrencyNumber.Value) / toCurrencyNumber.Value
    {
        //Arrange
        var amount = 5m;
        var fromCurrency = "EUR";
        var toCurrency = "USD";
        var fakeNbpResponse = new NbpRateResponse()
        {
            Rates = new List<NbpRate>
            {
                new NbpRate{Mid = 4.50m}
            }
        };
        var handler = new TestHttpMessageHandler(() => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(fakeNbpResponse)
        });
        var client = new HttpClient(handler);
        var currencyServices = new CurrencyService(client);
        //Act
        var rates = await currencyServices.ConvertAsync(amount, fromCurrency, toCurrency);
        //Assert
        Assert.Equal(5m, rates);
    }

    [Fact]
    public async Task Check_SpecialCase_NoApi()
    {
        //Arrange
        var baseCurrency = "PLN";
        var handler = new TestHttpMessageHandler(() => new HttpResponseMessage());
        var client = new HttpClient(handler);
        var currencyServices = new CurrencyService(client);
        //Act
        var rate = await currencyServices.GetRateAsync(baseCurrency);
        //Assert
        Assert.Equal(1, rate);
        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task Check_ErrorHandling_404()
    {
        var fakeCurrency = "GLORB";
        var handler = new TestHttpMessageHandler(() => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
        });
        var client = new HttpClient(handler);
        var currencyServices = new CurrencyService(client);
        //Act
        var checkForHandler = await currencyServices.GetRateAsync(fakeCurrency);
        //Assert
        Assert.Null(checkForHandler);
    }

    [Fact]
    public async Task Check_ErrorHandling_500()
    {
        var fakeCurrency = "GLORB";
        var handler = new TestHttpMessageHandler(() => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError,
        });
        var client = new HttpClient(handler);
        var currencyServices = new CurrencyService(client);
        //Act
        var exception = await Assert.ThrowsAsync<CurrencyServiceException>(() => currencyServices.GetRateAsync(fakeCurrency));
        //Assert
        Assert.Contains("Błąd pobierania kursu:",  exception.Message);
    }

    [Fact]
    public async Task Check_CorrectRates_ValuesNotNull()
    {
        //Arrange
        var fromCurrency = "EUR";
        var toCurrency = "USD";
        var fakeNbpResponse = new NbpRateResponse()
        {
            Rates = new List<NbpRate>
            {
                new NbpRate{Mid = 4.50m}
            }
        };
        var handler = new TestHttpMessageHandler(() => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(fakeNbpResponse)
        });
        var client = new HttpClient(handler);
        var currencyServices = new CurrencyService(client);
        //Act
        var a = await currencyServices.GetRateAsync(fromCurrency);
        var b = await currencyServices.GetRateAsync(toCurrency);
        //Assert
        Assert.Equal(4.5m, a);
        Assert.Equal(4.50m, b);
    }
    
    [Fact]
    public async Task Check_Path_CorrectUrl()
    {
        //Arrange
        var fromCurrency = "EUR";
        var expectedUrl = $"https://api.nbp.pl/api/exchangerates/rates/A/{fromCurrency}/?format=json";
        var fakeNbpResponse = new NbpRateResponse()
        {
            Rates = new List<NbpRate>
            {
                new NbpRate { Mid = 4.50m }
            }
        };
        var handler = new TestHttpMessageHandler(() => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(fakeNbpResponse)
        });
        var client = new HttpClient(handler);
        var currencyServices = new CurrencyService(client);
        //Act
        await currencyServices.GetRateAsync(fromCurrency);
        //Assert
        Assert.NotNull(handler.CorrectUri);
        Assert.Equal(expectedUrl, handler.CorrectUri.ToString());
    }
}