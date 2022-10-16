using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WeatherData.Service;

namespace Api;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var path = Path.GetFullPath(Path.Combine(builder.GetContext().ApplicationRootPath, "sample-data/weather.json"));
        builder.Services.AddScoped<IWeatherDataAccessService>(_ => new WeatherDataAccessService(path));
    }
}