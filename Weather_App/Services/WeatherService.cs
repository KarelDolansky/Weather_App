namespace Weather_App.Services
{
    public interface IWeatherService
    {
        WeatherData GetWeather(string location);
        WeatherData JsonGetWeather(string jsonString);
    }

    public class WeatherService : IWeatherService
    {
        private readonly bool TEST = true;
        private readonly IWeatherServiceHandler _weatherServiceHandler;
        private readonly IPositionServiceHandler _positionServiceHandler;
        public WeatherService(IWeatherServiceHandler weatherServiceHandler, IPositionServiceHandler positionServiceHandler)
        {
            _weatherServiceHandler = weatherServiceHandler;
            _positionServiceHandler = positionServiceHandler;
        }
        public WeatherData GetWeather(string location)
        {
            if(TEST)
            {
                var jsonString = "{\"latitude\":50.760002,\"longitude\":15.059999,\"generationtime_ms\":0.041961669921875,\"utc_offset_seconds\":7200,\"timezone\":\"Europe/Berlin\",\"timezone_abbreviation\":\"CEST\",\"elevation\":355,\"hourly_units\":{\"time\":\"iso8601\",\"temperature_2m\":\"°C\",\"precipitation\":\"mm\",\"rain\":\"mm\",\"showers\":\"mm\",\"snowfall\":\"cm\",\"snow_depth\":\"m\"},\"hourly\":{\"time\":[\"2024-04-15T00:00\",\"2024-04-15T01:00\",\"2024-04-15T02:00\",\"2024-04-15T03:00\",\"2024-04-15T04:00\",\"2024-04-15T05:00\",\"2024-04-15T06:00\",\"2024-04-15T07:00\",\"2024-04-15T08:00\",\"2024-04-15T09:00\",\"2024-04-15T10:00\",\"2024-04-15T11:00\",\"2024-04-15T12:00\",\"2024-04-15T13:00\",\"2024-04-15T14:00\",\"2024-04-15T15:00\",\"2024-04-15T16:00\",\"2024-04-15T17:00\",\"2024-04-15T18:00\",\"2024-04-15T19:00\",\"2024-04-15T20:00\",\"2024-04-15T21:00\",\"2024-04-15T22:00\",\"2024-04-15T23:00\"],\"temperature_2m\":[10.9,10,8.9,8.6,9,8.9,8.7,8,7.6,8,8.4,8.5,10.8,12.2,11.6,12.6,13.7,13.7,13.3,12.5,11.5,11.1,11.3,10.6],\"precipitation\":[0,0,0,0,0,0.1,0,0.6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.9,4.3],\"rain\":[0,0,0,0,0,0,0.4,0.2,0,0,0.1,0,0,0,0,0,0,0,0,0,0,0,0.4,0.1],\"showers\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.1],\"snowfall\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"snow_depth\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]}}";
                return System.Text.Json.JsonSerializer.Deserialize<WeatherData>(jsonString);
            }
            //trasformace z location na lat a lon
            PositionData position = _positionServiceHandler.CallApi(location).Result;
            //ziskani dat z api
            return _weatherServiceHandler.CallApi(position.results[0].latitude, position.results[0].longitude).Result;
        }
        public WeatherData JsonGetWeather(string jsonString)
        {
            return _weatherServiceHandler.JsonToWeatherData(jsonString);
        }
    }
}
