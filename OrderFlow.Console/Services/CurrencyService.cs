using System.Net;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace OrderFlow.Console.Services;

public interface ICurrencyService
{
    Task<decimal?> GetRateAsync(string currencyCode);
    Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);
}

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;

    public CurrencyService(HttpClient client)
    {
        _httpClient = client;
    }

    public async Task<decimal?> GetRateAsync(string currencyCode)
    {
        if (currencyCode.ToUpper() == "PLN")
        {
            return 1.0m;
        }

        var url = $"https://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<NbpRateResponse>(url);
            return response?.Rates.FirstOrDefault()?.Mid;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (HttpRequestException ex) 
        {
            throw new CurrencyServiceException($"Błąd pobierania kursu: {ex.Message}", ex);
        }
        catch (TaskCanceledException)
        {
            throw new CurrencyServiceException("Timeout przy pobieraniu kursu");
        }
    }
    
    public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
    {
        var fromCurrencyNumber = await GetRateAsync(fromCurrency);
        if (fromCurrencyNumber == null)
        {
            throw new CurrencyServiceException("Błąd pobierania kursu.");
        }
        var toCurrencyNumber = await GetRateAsync(toCurrency);
        if (toCurrencyNumber == null)
        {
            throw new CurrencyServiceException("Błąd pobierania kursu.");
        }
        var convertedAmount = (amount * fromCurrencyNumber.Value) / toCurrencyNumber.Value;
        return convertedAmount;
    }
}

[Serializable]
public class CurrencyServiceException : Exception
{
    public CurrencyServiceException(string message) : base(message)
    {}

    public CurrencyServiceException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}

public class NbpRateResponse
{
    [JsonPropertyName("table")]
    public string Table { get; set; } = "";

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "";

    [JsonPropertyName("code")]
    public string Code { get; set; } = "";

    [JsonPropertyName("rates")]
    public List<NbpRate> Rates { get; set; } = new();
}

public class NbpRate
{
    [JsonPropertyName("no")]
    public string No { get; set; } = "";

    [JsonPropertyName("effectiveDate")]
    public DateOnly EffectiveDate { get; set; }

    [JsonPropertyName("mid")]
    public decimal Mid { get; set; }
}
