using Microsoft.Extensions.Caching.Memory;

namespace Weather_App.Services
{
    public interface IWeatherService
    {
        WeatherData GetWeather(string location);
        string GetJson(string location);
        WeatherData GetWeather(string location, float latitude, float longitude);
    }

    public class WeatherService : IWeatherService
    {
        private readonly IWeatherServiceHandler _weatherServiceHandler;
        private readonly IPositionServiceHandler _positionServiceHandler;
        private readonly IWeatherDataTransformations _weatherDataTransformations;
        private readonly IMemoryCache _cache;
        public WeatherService(IWeatherServiceHandler weatherServiceHandler, IPositionServiceHandler positionServiceHandler,
            IWeatherDataTransformations weatherDataTransformations, IMemoryCache cache)
        {
            _weatherServiceHandler = weatherServiceHandler;
            _positionServiceHandler = positionServiceHandler;
            _weatherDataTransformations = weatherDataTransformations;
            _cache = cache;
        }
        
        public WeatherData GetWeather(string location)
        {
            string cachedData = _cache.Get<string>(location);
            if (cachedData != null)
            {
                return _weatherDataTransformations.JsonToWeatherData(cachedData); ;
            }
            PositionData position = _positionServiceHandler.CallApi(location).Result;
            string weatherJson = _weatherServiceHandler.CallApi(position.results[0].latitude, position.results[0].longitude).Result;
            _cache.Set(location, weatherJson, TimeSpan.FromSeconds(60));
            WeatherData weatherData = _weatherDataTransformations.JsonToWeatherData(weatherJson);
            return weatherData;
        }

        public string GetJson(string location)
        {
            string cachedData = _cache.Get<string>(location);
            if (cachedData != null)
            {
                return cachedData;
            }
            PositionData position = _positionServiceHandler.CallApi(location).Result;
            string weatherJson = _weatherServiceHandler.CallApi(position.results[0].latitude, position.results[0].longitude).Result;
            _cache.Set(location, weatherJson, TimeSpan.FromSeconds(60));
            return weatherJson;
        }
        public WeatherData GetWeather(string location, float latitude,float longitude)
        {
            string cachedData = _cache.Get<string>(location);
            if (cachedData != null)
            {
                return _weatherDataTransformations.JsonToWeatherData(cachedData); ;
            }
            string weatherJson = _weatherServiceHandler.CallApi(latitude, longitude).Result;
            _cache.Set(location, weatherJson, TimeSpan.FromSeconds(60));
            WeatherData weatherData = _weatherDataTransformations.JsonToWeatherData(weatherJson);
            return weatherData;
        }
    }
}
