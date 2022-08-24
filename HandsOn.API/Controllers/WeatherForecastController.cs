using HandsOn.API.Extensions;
using HandsOn.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace HandsOn.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IDistributedCache _cache;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IDistributedCache cache,
            ILogger<WeatherForecastController> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<object> Get()
        {
            WeatherForecast[]? forecasts = null;
            string? loadLocation = null;

            //Declaring a unit recordKey to set our get the data
            string recordKey = $"WeatherForecast_{DateTime.Now.ToString("yyyyMMdd_hhmm")}";

            forecasts = await _cache.GetRecordAsync<WeatherForecast[]>(recordKey);

            if (forecasts is not null)
            {
                loadLocation = $"Loaded from the cache at {DateTime.Now}";

                return new { LoadLocation = loadLocation, Forecasts = forecasts };
            }

            await Task.Delay(1500);

            forecasts = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();

            loadLocation = $"Loaded from the Database at {DateTime.Now}";

            await _cache.SetRecordAsync(recordKey, forecasts);

            return new { LoadLocation = loadLocation, Forecasts = forecasts };
        }
    }
}