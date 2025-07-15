using NewRelic.Api.Agent;
using System.Text.Json;
using WebAPIWithNewRelic.Models;

namespace WebAPIWithNewRelic.Services;

public class WeatherService
{
    private readonly ILogger<WeatherService> _logger;
    private readonly HttpClient _httpClient;

    public WeatherService(ILogger<WeatherService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.weatherapi.com/v1/");
    }

    [Transaction]
    public async Task<Weather?> GetWeatherAsync(string location, bool getAQI = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(location, nameof(location));

        try
        {
            var aqiParam = getAQI ? "yes" : "no";

            var response = await _httpClient.GetAsync(
                $"current.json?key=<api-key>&q={location}&aqi={aqiParam}"
                );

            response.EnsureSuccessStatusCode();

            var responseRaw = await response.Content.ReadAsStringAsync();
            var weather = JsonSerializer.Deserialize<Weather>(responseRaw);

            var agent = NewRelic.Api.Agent.NewRelic.GetAgent();
            NewRelic.Api.Agent.NewRelic.SetTransactionName("Services", nameof(WeatherService));
            ITransaction? transaction = agent.CurrentTransaction;

            transaction.AddCustomAttribute(nameof(location), weather?.Location?.Name ?? "N/A");
            transaction.AddCustomAttribute(nameof(getAQI), aqiParam);
            transaction.AddCustomAttribute(nameof(weather.Current.Temperature), weather?.Current?.Temperature ?? 0m);

            return weather;

        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("An exception has occurred in {ServiceName}, {Message}, {StackTrace}",
                             nameof(WeatherService),
                             ex.Message,
                             ex.StackTrace
                             );

            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }



}
