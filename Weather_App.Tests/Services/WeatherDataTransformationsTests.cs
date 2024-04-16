using Weather_App.Services;

namespace Weather_App.Tests.Services
{
    public class WeatherDataTransformationsTests
    {
        [Fact]
        public void StringToWeatherData_Converts_JsonString_To_WeatherData_Object()
        {
            var jsonString = "{\"latitude\":50.760002,\"longitude\":15.059999,\"generationtime_ms\":0.041961669921875,\"utc_offset_seconds\":7200,\"timezone\":\"Europe/Berlin\",\"timezone_abbreviation\":\"CEST\",\"elevation\":355,\"hourly_units\":{\"time\":\"iso8601\",\"temperature_2m\":\"°C\",\"precipitation\":\"mm\",\"rain\":\"mm\",\"showers\":\"mm\",\"snowfall\":\"cm\",\"snow_depth\":\"m\"},\"hourly\":{\"time\":[\"2024-04-15T00:00\",\"2024-04-15T01:00\",\"2024-04-15T02:00\",\"2024-04-15T03:00\",\"2024-04-15T04:00\",\"2024-04-15T05:00\",\"2024-04-15T06:00\",\"2024-04-15T07:00\",\"2024-04-15T08:00\",\"2024-04-15T09:00\",\"2024-04-15T10:00\",\"2024-04-15T11:00\",\"2024-04-15T12:00\",\"2024-04-15T13:00\",\"2024-04-15T14:00\",\"2024-04-15T15:00\",\"2024-04-15T16:00\",\"2024-04-15T17:00\",\"2024-04-15T18:00\",\"2024-04-15T19:00\",\"2024-04-15T20:00\",\"2024-04-15T21:00\",\"2024-04-15T22:00\",\"2024-04-15T23:00\"],\"temperature_2m\":[10.9,10,8.9,8.6,9,8.9,8.7,8,7.6,8,8.4,8.5,10.8,12.2,11.6,12.6,13.7,13.7,13.3,12.5,11.5,11.1,11.3,10.6],\"precipitation\":[0,0,0,0,0,0.1,0,0.6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.9,4.3],\"rain\":[0,0,0,0,0,0,0.4,0.2,0,0,0.1,0,0,0,0,0,0,0,0,0,0,0,0.4,0.1],\"showers\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.1],\"snowfall\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"snow_depth\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]}}";
            var weatherDataTransformations = new WeatherDataTransformations();
            var data = weatherDataTransformations.StringToWeatherData(jsonString);
            Assert.IsType<WeatherData>(data);
            Assert.Equal(10.9, data.hourly.temperature_2m[0]);
            Assert.All(data.hourly.temperature_2m, x => Assert.True(x >= 0));
        }
    }
}
