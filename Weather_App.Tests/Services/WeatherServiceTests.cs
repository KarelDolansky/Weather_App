using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Moq;
using Weather_App.Services;
using Xunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Text;
using Weather_App.Models;

public class WeatherServiceTests
{
    private readonly Mock<IWeatherServiceHandler> _weatherServiceHandlerMock;
    private readonly Mock<IPositionServiceHandler> _positionServiceHandlerMock;
    private readonly Mock<IWeatherDataTransformations> _weatherDataTransformationsMock;
    private readonly IMemoryCache _cache;
    private readonly WeatherService _weatherService;

    public WeatherServiceTests()
    {
        _weatherServiceHandlerMock = new Mock<IWeatherServiceHandler>();
        _positionServiceHandlerMock = new Mock<IPositionServiceHandler>();
        _weatherDataTransformationsMock = new Mock<IWeatherDataTransformations>();
        _cache = new FakeMemoryCache();
        _weatherService = new WeatherService(_weatherServiceHandlerMock.Object, _positionServiceHandlerMock.Object, _weatherDataTransformationsMock.Object, _cache);
    }

    [Fact]
    public void GetWeather_ThrowsException_WhenLocationIsNotFound()
    {
        // Arrange
        string location = null;
        var latitude = 50.760002f;
        var longitude = 15.059999f;
        var date = DateOnly.FromDateTime(DateTime.Now);


        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetWeather(location, latitude, longitude, date));
    }

    [Fact]
    public void GetWeather_ThrowsException_WhenLocationIsNotFoundV2()
    {
        // Arrange
        string location = null;
        var date = DateOnly.FromDateTime(DateTime.Now);


        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetWeather(location, date));
    }

    [Fact]
    public void GetWeather_ThrowsException_WhenWeatherDataIsInvalid()
    {
        // Arrange
        var location = "test_location";
        var date = DateOnly.FromDateTime(DateTime.Now);

        _positionServiceHandlerMock.Setup(p => p.CallApi(location)).ReturnsAsync((PositionData?)null);

        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetWeather(location,date));
    }

    [Fact]
    public void GetWeather_ThrowsException_WhenWeatherDataIsInvalidV2()
    {
        // Arrange
        var location = "test_location";
        var latitude = 50.760002f;
        var longitude = 15.059999f;
        var date = DateOnly.FromDateTime(DateTime.Now);

        _positionServiceHandlerMock.Setup(p => p.CallApi(location)).ReturnsAsync((PositionData?)null);

        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetWeather(location,latitude,longitude, date));
    }

    public void GetWeather_ThrowsException_WhenPositionDataIsInvalid()
    {
        // Arrange
        var location = "test_location";
        var date = DateOnly.FromDateTime(DateTime.Now);
        var positionData = new PositionData(new List<Results> { new Results("test_location", 50.760002, 15.059999) });
        var weatherJson = "{\"latitude\":50.760002,\"longitude\":15.059999,\"hourly\":{},\"daily\":{}}";

        _positionServiceHandlerMock.Setup(p => p.CallApi(location)).ReturnsAsync(positionData);
        _weatherServiceHandlerMock.Setup(w => w.CallApi(positionData.results[0].latitude, positionData.results[0].longitude, date)).ReturnsAsync((string?)null);

        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetWeather(location, date));
    }

    [Fact]
    public void GetWeather_ThrowsException_WhenPositionDataIsInvalidV2()
    {
        // Arrange
        var location = "test_location";
        var latitude = 50.760002f;
        var longitude = 15.059999f;
        var date = DateOnly.FromDateTime(DateTime.Now);
        var positionData = new PositionData(new List<Results> { new Results("test_location", 50.760002, 15.059999) });
        var weatherJson = "{\"latitude\":50.760002,\"longitude\":15.059999,\"hourly\":{},\"daily\":{}}";

        _positionServiceHandlerMock.Setup(p => p.CallApi(location)).ReturnsAsync(positionData);
        _weatherServiceHandlerMock.Setup(w => w.CallApi(positionData.results[0].latitude, positionData.results[0].longitude, date)).ReturnsAsync((string?)null);

        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetWeather(location, latitude, longitude, date));
    }



    [Fact]
    public void GetWeather_WithCoordinates_ReturnsWeatherData_WhenLocationIsFound()
    {
        // Arrange
        var location = "test_location";
        var latitude = 50.760002f;
        var longitude = 15.059999f;
        var date = DateOnly.FromDateTime(DateTime.Now);
        var weatherJson = "{\"latitude\":50.760002,\"longitude\":15.059999,\"hourly\":{},\"daily\":{}}";
        var weatherData = new WeatherData(50.760002, 15.059999, new Hourly(new List<string>(), new List<double>(), new List<double>(), new List<double>(), new List<double>(), new List<double>(), new List<double>()), new Daily(new List<int>()));

        _weatherServiceHandlerMock.Setup(w => w.CallApi(latitude, longitude, date)).ReturnsAsync(weatherJson);
        _weatherDataTransformationsMock.Setup(w => w.JsonToWeatherData(weatherJson)).Returns(weatherData);

        // Act
        var result = _weatherService.GetWeather(location, latitude, longitude, date);

        // Assert
        Assert.Equal(weatherData, result);
    }

    [Fact]
    public void GetWeather_ReturnsWeatherData_WhenLocationIsFound()
    {
        // Arrange
        var location = "test_location";
        var latitude = 50.760002f;
        var longitude = 15.059999f;
        var date = DateOnly.FromDateTime(DateTime.Now);
        var weatherJson = "{\"latitude\":50.760002,\"longitude\":15.059999,\"hourly\":{},\"daily\":{}}";
        var weatherData = new WeatherData(50.760002, 15.059999, new Hourly(new List<string>(), new List<double>(), new List<double>(), new List<double>(), new List<double>(), new List<double>(), new List<double>()), new Daily(new List<int>()));

        _weatherServiceHandlerMock.Setup(w => w.CallApi(latitude, longitude, date)).ReturnsAsync(weatherJson);
        _weatherDataTransformationsMock.Setup(w => w.JsonToWeatherData(weatherJson)).Returns(weatherData);

        // Act
        var result = _weatherService.GetWeather(location, latitude, longitude, date);

        // Assert
        Assert.Equal(weatherData, result);
    }
    [Fact]
    public void GetJson_ReturnsJson_WhenLocationIsFound()
    {
        // Arrange
        var location = "test_location";
        var date = DateOnly.FromDateTime(DateTime.Now);
        var positionData = new PositionData(new List<Results> { new Results("test_location", 50.760002, 15.059999) });
        var weatherJson = "{\"latitude\":50.760002,\"longitude\":15.059999,\"hourly\":{},\"daily\":{}}";

        _positionServiceHandlerMock.Setup(p => p.CallApi(location)).ReturnsAsync(positionData);
        _weatherServiceHandlerMock.Setup(w => w.CallApi(positionData.results[0].latitude, positionData.results[0].longitude, date)).ReturnsAsync(weatherJson);

        var result = _weatherService.GetJson(location);
        Assert.Equal(weatherJson, result);
    }

    [Fact]
    public void GetWeather_ReturnsWeather_WhenLocationIsFound()
    {
        // Arrange
        var location = "test_location";
        var date = DateOnly.FromDateTime(DateTime.Now);
        var positionData = new PositionData(new List<Results> { new Results("test_location", 50.760002, 15.059999) });
        var weatherJson = "{\"latitude\":50.760002,\"longitude\":15.059999,\"hourly\":{},\"daily\":{}}";
        var weather = new WeatherData(50.760002, 15.059999, new Hourly(new List<string>(), new List<double>(), new List<double>(), new List<double>(), new List<double>(), new List<double>(), new List<double>()), new Daily(new List<int>()));

        _positionServiceHandlerMock.Setup(p => p.CallApi(location)).ReturnsAsync(positionData);
        _weatherServiceHandlerMock.Setup(w => w.CallApi(positionData.results[0].latitude, positionData.results[0].longitude, date)).ReturnsAsync(weatherJson);
        _weatherDataTransformationsMock.Setup(w => w.JsonToWeatherData(weatherJson)).Returns(weather);

        var result = _weatherService.GetWeather(location, date);
        Assert.Equal(weather, result);
    }

    [Fact]
    public void GetJson_ThrowsException_WhenLocationIsNotFound()
    {
        // Arrange
        var location = "test_location";

        _positionServiceHandlerMock.Setup(p => p.CallApi(location)).ReturnsAsync((PositionData?)null);

        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetJson(location));
    }
    [Fact]
    public void GetJson_ThrowsException_WhenLocationIsNotFoundV2()
    {
        // Arrange
        string location = null;

        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetJson(location));
    }

    [Fact]
    public void GetJson_ThrowsException_WhenWeatherDataIsNotFound()
    {
        // Arrange
        var location = "test_location";
        var date = DateOnly.FromDateTime(DateTime.Now);
        var positionData = new PositionData(new List<Results> { new Results("test_location", 50.760002, 15.059999) });

        _positionServiceHandlerMock.Setup(p => p.CallApi(location)).ReturnsAsync(positionData);
        _weatherServiceHandlerMock.Setup(w => w.CallApi(positionData.results[0].latitude, positionData.results[0].longitude, date)).ReturnsAsync((string?)null);

        // Act & Assert
        Assert.Throws<ExceptionBadRequest>(() => _weatherService.GetJson(location));
    }
    [Fact]
    public void WeatherCode_ShouldCreateInstance_AndAccessProperties()
    {
        // Arrange
        var weatherConditions = new List<WeatherConditions>
    {
        new WeatherConditions(100, "Clear sky", "sunny.png")
    };

        // Act
        var weatherCode = new WeatherCode(weatherConditions);

        // Assert
        Assert.Equal(weatherConditions, weatherCode.weather_conditions);
        Assert.Equal(100, weatherCode.weather_conditions[0].code);
        Assert.Equal("Clear sky", weatherCode.weather_conditions[0].description);
        Assert.Equal("sunny.png", weatherCode.weather_conditions[0].image);
    }


}

public class FakeMemoryCache : IMemoryCache
{
    private readonly Dictionary<object, object?> _cache = new Dictionary<object, object?>();

    public ICacheEntry CreateEntry(object key)
    {
        return new FakeCacheEntry(key, _cache);
    }

    public void Dispose()
    {
    }

    public void Remove(object key)
    {
        _cache.Remove(key);
    }

    public bool TryGetValue(object key, out object? value)
    {
        return _cache.TryGetValue(key, out value);
    }
}

public class FakeCacheEntry : ICacheEntry
{
    private readonly object _key;
    private readonly Dictionary<object, object?> _cache;

    public FakeCacheEntry(object key, Dictionary<object, object?> cache)
    {
        _key = key;
        _cache = cache;
    }

    public object Key => _key;
    public object? Value { get => _cache[_key]; set => _cache[_key] = value; }
    public DateTimeOffset? AbsoluteExpiration { get; set; }
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }
    public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();
    public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<PostEvictionCallbackRegistration>();
    public CacheItemPriority Priority { get; set; }
    public long? Size { get; set; }

    public void Dispose()
    {
    }
}
