namespace Weather_App.Services
{
    public interface IWeatherService
    {
        WeatherData GetWeather(string location);
    }

    public class WeatherService : IWeatherService
    {
        private readonly IWeatherServiceHandler _weatherServiceHandler;
        private readonly IPositionServiceHandler _positionServiceHandler;
        public WeatherService(IWeatherServiceHandler weatherServiceHandler, IPositionServiceHandler positionServiceHandler)
        {
            _weatherServiceHandler = weatherServiceHandler;
            _positionServiceHandler = positionServiceHandler;
        }
        public WeatherData GetWeather(string location)
        {
            //trasformace z location na lat a lon
            PositionData position = _positionServiceHandler.CallApi(location).Result;
            //ziskani dat z api
            return _weatherServiceHandler.CallApi(position.results[0].latitude, position.results[0].longitude).Result;
        }
    }
}
