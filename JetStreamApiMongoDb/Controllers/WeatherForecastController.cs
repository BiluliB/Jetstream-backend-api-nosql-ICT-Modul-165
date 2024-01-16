using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace JetStreamApiMongoDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMongoDbContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMongoDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var status = await _context.Statuses.FindWithProxies(FilterDefinition<Status>.Empty);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
