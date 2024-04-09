using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Weather_App.Controllers;
using Weather_App.Models;
using Xunit;

namespace Weather_App.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsView()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(mockLogger.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.ViewData["WeatherData"]); // No weather data expected for Index
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(mockLogger.Object);

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
