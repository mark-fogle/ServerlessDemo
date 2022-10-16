using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using WeatherData.Models;
using WeatherData.Service;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Api
{
    public class Weather
    {
        private readonly ILogger<Weather> _logger;
        private readonly IWeatherDataAccessService _weatherDataAccessService;

        public Weather(ILogger<Weather> log, IWeatherDataAccessService weatherDataAccessService)
        {
            _logger = log ?? throw new ArgumentNullException(nameof(log));
            _weatherDataAccessService = weatherDataAccessService ?? throw new ArgumentNullException(nameof(weatherDataAccessService));
        }

        [FunctionName(nameof(Weather))]
        [OpenApiOperation(operationId: "GetWeather")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast[]), Description = "Weather Data")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ExecutionContext context)
        {
            _logger.LogInformation("GetWeather processed a request.");

            var weather = await _weatherDataAccessService.GetWeatherData().ToArrayAsync();

            return new OkObjectResult(weather);
        }
    }



}

