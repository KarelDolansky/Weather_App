using Weather_App.Models;
namespace Weather_App.Services
{
    public interface IIconWeather
    {
        string GetIcon(int code);
    }
    public class IconWeather : IIconWeather
    {
        private readonly WeatherCode _weathercode;
        public IconWeather()
        {
            string json = System.IO.File.ReadAllText("wwwroot/weather.json");
            _weathercode = System.Text.Json.JsonSerializer.Deserialize<WeatherCode>(json!);
        }
        public string GetIcon(int code)
        {
            return _weathercode.weather_conditions.Find(x => x.code == code).image;
        }
    }
}
