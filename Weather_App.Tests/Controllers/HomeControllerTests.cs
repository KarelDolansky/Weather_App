using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Weather_App.Controllers;
using Weather_App.Models;
using Weather_App.Services;
using Xunit;

namespace Weather_App.Tests.Controllers
{
    public class HomeControllerTests
    {
        // Simulate the IWeatherService dependency using a mock
        private readonly Mock<IWeatherService> _mockWeatherService = new Mock<IWeatherService>();
        private readonly Mock<ILogger<HomeController>> _mockLogger = new Mock<ILogger<HomeController>>();

        [Fact]
        public void Index_ReturnsViewWithCorrectTitle()
        {
            // Arrange
            var controller = new HomeController(_mockLogger.Object, _mockWeatherService.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Poèasí", viewResult.ViewData["Title"] as string);
        }

        [Fact]
        public void Weather_ReturnsViewWithWeatherData_WhenLocationProvided()
        {
            // Arrange
            var expectedWeatherData = new WeatherData(10, 10, hourly: new Hourly(null, temperature_2m: new List<double> { 10.9 }, null, null, null, null, null));
            var location = "Prague";
            _mockWeatherService.Setup(service => service.GetWeather(location)).Returns(expectedWeatherData);

            var controller = new HomeController(_mockLogger.Object, _mockWeatherService.Object);

            // Act
            var result = controller.Weather(location);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedWeatherData, viewResult.ViewData["weatherData"] as WeatherData);
        }

        [Fact]
        public void Weather_ReturnsViewWithEmptyWeatherData_WhenLocationIsNull()
        {
            // Arrange
            var controller = new HomeController(_mockLogger.Object, _mockWeatherService.Object);

            // Act
            var result = controller.Weather(null);

            // Assert (consider appropriate assertion based on your implementation)
            var viewResult = Assert.IsType<ViewResult>(result);
            // Option 1: Assert that weatherData is null or empty
            Assert.Null(viewResult.ViewData["weatherData"]);
            // Option 2: Assert for a specific error message or model state (if applicable)
            // ...
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            // Arrange
            var controller = new HomeController(null, null); // Mocks not needed for Privacy

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewWithRequestId()
        {
            // Arrange
            var controller = new HomeController(null, null); // Mocks not needed for Error

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.NotNull(model.RequestId);
        }
    }
}
