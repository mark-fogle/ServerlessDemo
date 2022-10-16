using WeatherData.Models;

namespace WeatherData.Service;

public interface IWeatherDataAccessService
{
    public IAsyncEnumerable<WeatherForecast> GetWeatherData();
}