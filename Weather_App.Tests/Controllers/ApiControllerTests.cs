using Moq;
using Weather_App.Services;
using Weather_App.Controllers;

namespace Weather_App.Tests.Controllers
{
    public class ApiControllerTests
    {
        [Fact]
        public void Get_ReturnsWeatherJson_ForValidLocation()
        {
            // Arrange
            var mockWeatherService = new Mock<IWeatherService>();
            string expectedWeatherJson = "{'temperature': 20, 'conditions': 'Sunny'}";
            mockWeatherService.Setup(s => s.GetJson(It.IsAny<string>())).Returns(expectedWeatherJson);

            var controller = new ApiController(mockWeatherService.Object);

            // Act
            var result = controller.Get("Prague");
            
            // Assert
            Assert.Equal(expectedWeatherJson, result);
        }
    }
}
