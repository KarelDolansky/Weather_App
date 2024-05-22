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
    public void Index_ShouldReturnViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }


    [Fact]
    public void AddFavorite_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = _controller.AddFavorite("Prague", 50.0755f, 14.4378f);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void RemoveFavorite_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = _controller.RemoveFavorite("Prague");

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void AddFavorite_ShouldReturnRedirectToActionResult_WhenUserIsAuthenticated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "username") }, "mock"));
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        // Act
        var result = _controller.AddFavorite("Prague", 50.0755f, 14.4378f);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public void RemoveFavorite_ShouldReturnRedirectToActionResult_WhenUserIsAuthenticated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "username") }, "mock"));
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        // Act
        var result = _controller.RemoveFavorite("Prague");

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }


}
