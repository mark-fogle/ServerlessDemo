using Newtonsoft.Json;
using WeatherData.Models;

namespace WeatherData.Service
{
    public class WeatherDataAccessService : IWeatherDataAccessService
    {
        private readonly string _dataPath;

        public WeatherDataAccessService(string dataPath)
        {
            _dataPath = dataPath;
        }

        public async IAsyncEnumerable<WeatherForecast> GetWeatherData()
        {
            var weatherJson = await File.ReadAllTextAsync(_dataPath);

            var entries = JsonConvert.DeserializeObject<WeatherForecast[]>(weatherJson);

            if (entries == null)
                yield break;

            foreach (var entry in entries)
            {
                yield return entry;
            }
        }
    }
}