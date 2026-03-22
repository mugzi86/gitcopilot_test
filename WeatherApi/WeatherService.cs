using Microsoft.Extensions.Caching.Memory;

namespace WeatherApi;

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC * 9 / 5.0);
}

public class WeatherService
{
    private readonly ILogger<WeatherService> _logger;
    private readonly IMemoryCache _cache;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherService(ILogger<WeatherService> logger, IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public WeatherForecast[] GetWeatherForecast()
    {
        const string cacheKey = "weatherforecast";
        if (_cache.TryGetValue(cacheKey, out WeatherForecast[] forecast))
        {
            _logger.LogInformation("Returning cached weather forecast");
            return forecast;
        }

        _logger.LogInformation("Generating new weather forecast");
        forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();

        _cache.Set(cacheKey, forecast, TimeSpan.FromMinutes(5));
        return forecast;
    }
}