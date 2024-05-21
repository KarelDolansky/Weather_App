using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Weather_App.Controllers;
using Weather_App.Models;
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
    public async Task AddFavorite_ReturnsRedirectToAction_WhenLocationProvided()
    {
        // Arrange
        var location = "Prague";
        var latitude = 50f;
        var longitude = 10f;
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "username") }));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        _mockFavoriteService.Setup(service => service.Add(location, latitude, longitude, user)).Returns(Task.CompletedTask);

        // Act
        var result = _controller.AddFavorite(location, latitude, longitude);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }



    [Fact]
    public void RemoveFavorite_ReturnsRedirectToAction_WhenLocationProvided()
    {
        // Arrange
        var location = "Prague";
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "username") }));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        _mockFavoriteService.Setup(service => service.Remove(location,user)).Returns(Task.CompletedTask);

        // Act
        var result = _controller.RemoveFavorite(location);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public void Favorite_ReturnsPartialView_WhenUserIsAuthenticated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "username"),
        }));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };


        var favorites = new List<Favorite>
        {
            new Favorite { Location = "Prague", Latitude = 50.0755f, Longitude = 14.4378f }
        };

        _mockFavoriteService.Setup(service => service.GetAll(user)).ReturnsAsync(favorites);



        // Act
        var result = _controller.Favorite();

        // Assert
        var viewResult = Assert.IsType<PartialViewResult>(result);
        Assert.Equal("_Favorite", viewResult.ViewName);
        Assert.Equal(favorites, viewResult.ViewData["favorites"] as List<Favorite>);
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
