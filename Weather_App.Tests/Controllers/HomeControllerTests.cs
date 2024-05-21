using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Weather_App.Controllers;
using Weather_App.Services;

public class HomeControllerTests
{
    private readonly Mock<IWeatherService> _mockWeatherService;
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly Mock<IFavoriteService> _mockFavoriteService;
    private readonly Mock<IIconWeather> _mockIconWeather;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _mockWeatherService = new Mock<IWeatherService>();
        _mockLogger = new Mock<ILogger<HomeController>>();
        _mockFavoriteService = new Mock<IFavoriteService>();
        _mockIconWeather = new Mock<IIconWeather>();
        _controller = new HomeController(_mockLogger.Object, _mockWeatherService.Object, _mockFavoriteService.Object, _mockIconWeather.Object);
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        // Act
        var result = _controller.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    // Your existing Weather method tests go here

    [Fact]
    public void WeatherMore_ReturnsViewResult_WhenLocationProvided()
    {
        // Arrange
        var location = "Prague";
        var date = DateOnly.FromDateTime(DateTime.Now);
        var expectedWeatherData = new WeatherData(10, 10, new Hourly(new List<string> { "10.9" }, null, null, null, null, null, null), new Daily(null));
        _mockWeatherService.Setup(service => service.GetWeather(location, date)).Returns(expectedWeatherData);

        // Act
        var result = _controller.WeatherMore(location, null, null, date);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(expectedWeatherData, viewResult.ViewData["weatherData"] as WeatherData);
    }

    [Fact]
    public void Favorite_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = _controller.Favorite();

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }
}
