using System.Text.Json;

namespace Weather_App.Services
{
    public interface IWeatherDataTransformations
    {
        WeatherData JsonToWeatherData(string jsonString);
    }
    public class WeatherDataTransformations:IWeatherDataTransformations
    {
        public WeatherData JsonToWeatherData(string jsonString)
        {
            WeatherData data = JsonSerializer.Deserialize<WeatherData>(jsonString);
            return data;
        }
    }
}
