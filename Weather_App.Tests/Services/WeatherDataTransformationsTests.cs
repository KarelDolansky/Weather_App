using System.Text.Json;
using Weather_App.Services;

public class WeatherDataTransformationsTests
{
    private readonly WeatherDataTransformations _weatherDataTransformations;

    public WeatherDataTransformationsTests()
    {
        _weatherDataTransformations = new WeatherDataTransformations();
    }

    [Fact]
    public void JsonToWeatherData_Converts_JsonString_To_WeatherData_Object()
    {
        // Arrange
        var jsonString = "{\"latitude\":50.760002,\"longitude\":15.059999,\"hourly\":{\"time\":[\"2024-04-15T00:00\",\"2024-04-15T01:00\"],\"temperature_2m\":[10.9,10],\"precipitation\":[0,0],\"rain\":[0,0],\"showers\":[0,0],\"snowfall\":[0,0],\"snow_depth\":[0,0]},\"daily\":{\"weather_code\":[100,200]}}";

        // Act
        var data = _weatherDataTransformations.JsonToWeatherData(jsonString);

        // Assert
        Assert.IsType<WeatherData>(data);
        Assert.Equal(50.760002, data.latitude);
        Assert.Equal(15.059999, data.longitude);
        Assert.Equal(10.9, data.hourly.temperature_2m[0]);
        Assert.Equal(100, data.daily.weather_code[0]);
        Assert.All(data.hourly.temperature_2m, x => Assert.True(x >= 0));
        Assert.All(data.hourly.rain, x => Assert.True(x >= 0));
        Assert.All(data.hourly.precipitation, x => Assert.True(x >= 0));
        Assert.All(data.hourly.snow_depth, x => Assert.True(x >= 0));
        Assert.All(data.hourly.snowfall, x => Assert.True(x >= 0));
        Assert.All(data.hourly.showers, x => Assert.True(x >= 0));
        Assert.All(data.hourly.time, x => Assert.True(DateTime.TryParse(x, out _)));
        Assert.Equal(2, data.hourly.time.Count);
        Assert.Equal(2, data.daily.weather_code.Count);
    }

    [Fact]
    public void JsonToWeatherData_NullJson_ThrowsArgumentNullException()
    {
        // Arrange
        string jsonString = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => _weatherDataTransformations.JsonToWeatherData(jsonString));
    }
}
