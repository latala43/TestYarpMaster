using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestYarpMaster.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WMController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Master", "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<WMController> _logger;

        public WMController(IHttpClientFactory clientFactory, ILogger<WMController> logger)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetDeailt")]
        public async Task<IEnumerable<WeatherForecast>> GetDeailAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://testyarpdetail.dev/WD");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync
                    <IEnumerable<WeatherForecast>>(responseStream);
                return result;
            }

            return null;
        }
    }
}
