using System.Text.Json;

namespace Weather_App.Services
{
    public interface IWeatherDataTransformations
    {
        WeatherData StringToWeatherData(string jsonString);
    }
    public class WeatherDataTransformations:IWeatherDataTransformations
    {
        public WeatherData StringToWeatherData(string jsonString)
        {
            WeatherData data = JsonSerializer.Deserialize<WeatherData>(jsonString);
            return data;
        }
    }
}
