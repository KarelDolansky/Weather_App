using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Weather_App.Controllers;
using Weather_App.Services;
using Microsoft.AspNetCore.Mvc;
using Weather_App.Models;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _loggerMock;
    private readonly Mock<IWeatherService> _weatherServiceMock;
    private readonly Mock<IFavoriteService> _favoriteServiceMock;
    private readonly Mock<IIconWeather> _iconWeatherMock;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _loggerMock = new Mock<ILogger<HomeController>>();
        _weatherServiceMock = new Mock<IWeatherService>();
        _favoriteServiceMock = new Mock<IFavoriteService>();
        _iconWeatherMock = new Mock<IIconWeather>();
        _controller = new HomeController(_loggerMock.Object, _weatherServiceMock.Object, _favoriteServiceMock.Object, _iconWeatherMock.Object);
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        var result = _controller.Index();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        var result = _controller.Privacy();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Weather_InvalidLocation_ReturnsRedirectToError()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

        // Act
        var result = _controller.Weather(null, null, null, 0, DateOnly.FromDateTime(DateTime.Now));

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
    }

    [Fact]
    public void Weather_ValidLocation_ReturnsViewResult()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);
        var weatherData = new WeatherData(
            latitude: 10,
            longitude: 10,
            hourly: new Hourly(
                time: new List<string> { "10:00" },
                temperature_2m: new List<double> { 15.5 },
                precipitation: new List<double> { 0.0 },
                rain: new List<double> { 0.0 },
                showers: new List<double> { 0.0 },
                snowfall: new List<double> { 0.0 },
                snow_depth: new List<double> { 0.0 }
            ),
            daily: new Daily(
                weather_code: new List<int> { 100 }
            )
        );
        // Assume you have a WeatherData model

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), date))
            .Returns(weatherData);

        _iconWeatherMock.Setup(service => service.GetIcon(It.IsAny<int>()))
            .Returns("icon_path");

        // Act
        var result = _controller.Weather("Prague", null, null, 0, date);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(weatherData, viewResult.ViewData["weatherData"]);
        Assert.Equal("icon_path", viewResult.ViewData["Icon"]);
    }

    [Fact]
    public async Task Favorite_NotAuthenticated_ReturnsUnauthorizedResult()
    {
        // Arrange
        var userMock = new Mock<ClaimsPrincipal>();
        userMock.Setup(user => user.Identity.IsAuthenticated).Returns(false);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userMock.Object }
        };

        // Act
        var result =  _controller.Favorite();

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Favorite_Authenticated_ReturnsPartialViewResult()
    {
        // Arrange
        var userMock = new Mock<ClaimsPrincipal>();
        userMock.Setup(user => user.Identity.IsAuthenticated).Returns(true);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userMock.Object }
        };

        var favorites = new List<Favorite>(); // Assume you have a Favorite model
        _favoriteServiceMock.Setup(service => service.GetAll(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(favorites);

        // Act
        var result = _controller.Favorite();

        // Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        Assert.Equal("_Favorite", partialViewResult.ViewName);
        Assert.Equal(favorites, partialViewResult.Model);
    }

    [Fact]
    public void AddFavorite_NotAuthenticated_ReturnsUnauthorizedResult()
    {
        // Arrange
        var userMock = new Mock<ClaimsPrincipal>();
        userMock.Setup(user => user.Identity.IsAuthenticated).Returns(false);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userMock.Object }
        };

        // Act
        var result = _controller.AddFavorite("Prague", 50.0755f, 14.4378f);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void AddFavorite_Authenticated_RedirectsToIndex()
    {
        // Arrange
        var userMock = new Mock<ClaimsPrincipal>();
        userMock.Setup(user => user.Identity.IsAuthenticated).Returns(true);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userMock.Object }
        };

        // Act
        var result = _controller.AddFavorite("Prague", 50.0755f, 14.4378f);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Home", redirectResult.ControllerName);
    }

    [Fact]
    public void RemoveFavorite_NotAuthenticated_ReturnsUnauthorizedResult()
    {
        // Arrange
        var userMock = new Mock<ClaimsPrincipal>();
        userMock.Setup(user => user.Identity.IsAuthenticated).Returns(false);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userMock.Object }
        };

        // Act
        var result = _controller.RemoveFavorite("Prague");

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void RemoveFavorite_Authenticated_RedirectsToIndex()
    {
        // Arrange
        var userMock = new Mock<ClaimsPrincipal>();
        userMock.Setup(user => user.Identity.IsAuthenticated).Returns(true);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userMock.Object }
        };

        // Act
        var result = _controller.RemoveFavorite("Prague");

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Home", redirectResult.ControllerName);
    }

    [Fact]
    public void WeatherMore_InvalidLocation_ReturnsRedirectToError()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

        // Act
        var result = _controller.WeatherMore(null, null, null, DateOnly.FromDateTime(DateTime.Now));

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
    }

    [Fact]
    public void WeatherMore_ValidLocation_ReturnsViewResult()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);
        var weatherData = new WeatherData(
            latitude: 10,
            longitude: 10,
            hourly: new Hourly(
                time: new List<string> { "10:00" },
                temperature_2m: new List<double> { 15.5 },
                precipitation: new List<double> { 0.0 },
                rain: new List<double> { 0.0 },
                showers: new List<double> { 0.0 },
                snowfall: new List<double> { 0.0 },
                snow_depth: new List<double> { 0.0 }
            ),
            daily: new Daily(
                weather_code: new List<int> { 100 }
            )
        );

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), date))
            .Returns(weatherData);

        // Act
        var result = _controller.WeatherMore("Prague", null, null, date);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(weatherData, viewResult.ViewData["weatherData"]);
    }

    [Fact]
    public void WeatherMore_WeatherServiceThrowsExceptionBadRequest_ReturnsRedirectToError()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), date))
            .Throws(new ExceptionBadRequest());

        // Act
        var result = _controller.WeatherMore("Prague", null, null, date);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
    }
    [Fact]
    public void WeatherMore_WeatherServiceThrowsExceptionApiCall_ReturnsRedirectToError()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), date))
            .Throws(new ExceptionApiCall());

        // Act
        var result = _controller.WeatherMore("Prague", null, null, date);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
    }

    [Fact]
    public void WeatherMore_WeatherServiceThrowsGenericException_ReturnsRedirectToError()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), date))
            .Throws(new Exception());

        // Act
        var result = _controller.WeatherMore("Prague", null, null, date);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
    }

    [Fact]
    public void WeatherMore_ValidLocationAndCoordinates_ReturnsViewResult()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);
        var weatherData = new WeatherData(
             latitude: 10,
             longitude: 10,
             hourly: new Hourly(
                 time: new List<string> { "10:00" },
                 temperature_2m: new List<double> { 15.5 },
                 precipitation: new List<double> { 0.0 },
                 rain: new List<double> { 0.0 },
                 showers: new List<double> { 0.0 },
                 snowfall: new List<double> { 0.0 },
                 snow_depth: new List<double> { 0.0 }
             ),
             daily: new Daily(
                 weather_code: new List<int> { 100 }
             )
         );

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), It.IsAny<float>(), It.IsAny<float>(), date))
            .Returns(weatherData);

        // Act
        var result = _controller.WeatherMore("Prague", 50.0755f, 14.4378f, date);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(weatherData, viewResult.ViewData["weatherData"]);
    }
    [Fact]
    public void Weather_WeatherServiceThrowsExceptionBadRequest_ReturnsRedirectToError()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), date))
            .Throws(new ExceptionBadRequest());

        // Act
        var result = _controller.Weather("Prague", null, null, 0, date);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
        Assert.Equal("Špatnì zadané místo", _controller.TempData["ErrorMessage"]);
    }

    [Fact]
    public void Weather_WeatherServiceThrowsExceptionApiCall_ReturnsRedirectToError()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), date))
            .Throws(new ExceptionApiCall());

        // Act
        var result = _controller.Weather("Prague", null, null, 0, date);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
        Assert.Equal("Chyba pøi získávání dat o poèasí. Zkuste to prosím znovu.", _controller.TempData["ErrorMessage"]);
    }

    [Fact]
    public void Weather_WeatherServiceThrowsGenericException_ReturnsRedirectToError()
    {
        // Arrange
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        var date = DateOnly.FromDateTime(DateTime.Now);

        _weatherServiceMock.Setup(service => service.GetWeather(It.IsAny<string>(), date))
            .Throws(new Exception());

        // Act
        var result = _controller.Weather("Prague", null, null, 0, date);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
        Assert.Equal("Nastala neoèekávána chyba. Zkuste to prosím znovu.", _controller.TempData["ErrorMessage"]);
    }

    [Fact]
    public void WeatherMore_ValidDate_ReturnsViewResult()
    {
        // Arrange
        var validDate = DateOnly.FromDateTime(DateTime.Now);
        var location = "Prague";
        var weatherData = new WeatherData(
             latitude: 10,
             longitude: 10,
             hourly: new Hourly(
                 time: new List<string> { "10:00" },
                 temperature_2m: new List<double> { 15.5 },
                 precipitation: new List<double> { 0.0 },
                 rain: new List<double> { 0.0 },
                 showers: new List<double> { 0.0 },
                 snowfall: new List<double> { 0.0 },
                 snow_depth: new List<double> { 0.0 }
             ),
             daily: new Daily(
                 weather_code: new List<int> { 100 }
             )
         );
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

        _weatherServiceMock.Setup(service => service.GetWeather(location, validDate))
            .Returns(weatherData);

        // Act
        var result = _controller.WeatherMore(location, null, null, validDate);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(weatherData, viewResult.ViewData["weatherData"]);
        Assert.Equal(location, viewResult.ViewData["location"]);
    }

}
