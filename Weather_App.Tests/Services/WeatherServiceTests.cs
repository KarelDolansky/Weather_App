//using Moq;
//using Microsoft.Extensions.Caching.Memory;
//using Weather_App.Services;

//namespace Weather_App.Tests
//{
//    public class WeatherServiceTests
//    {
//        [Fact]
//        public void GetWeather_Returns_Cached_Data_If_Available()
//        {
//            // Arrange
//            var cacheMock = new Mock<IMemoryCache>();
//            var cachedWeatherData = new WeatherData(10, 10, hourly: new Hourly(null, temperature_2m: new List<double> { 10.9 }, null, null, null, null, null));
//            cacheMock.Setup(cache => cache.Get<WeatherData>(It.IsAny<string>())).Returns(cachedWeatherData);

//            var weatherServiceHandlerMock = new Mock<IWeatherServiceHandler>();
//            var positionServiceHandlerMock = new Mock<IPositionServiceHandler>();
//            var weatherDataTransformationsMock = new Mock<IWeatherDataTransformations>();

//            var service = new WeatherService(
//                weatherServiceHandlerMock.Object,
//                positionServiceHandlerMock.Object,
//                weatherDataTransformationsMock.Object,
//                cacheMock.Object
//            );

//            // Act
//            var result = service.GetWeather("test_location");

//            // Assert
//            Assert.Equal(cachedWeatherData, result);
//            cacheMock.Verify(cache => cache.Get<WeatherData>("test_location"), Times.Once);
//        }

//        [Fact]
//        public void GetWeather_Calls_Api_When_Data_Not_Cached()
//        {
//            // Arrange
//            var cacheMock = new Mock<IMemoryCache>();
//            cacheMock.Setup(cache => cache.Get<WeatherData>(It.IsAny<string>())).Returns((string key) => null);

//            var positionData = new PositionData(new List<Results> { new Results("Liberec", 10, 10) });
//            var weatherJson = "test_weather_json";
//            var weatherData = new WeatherData(10, 10, hourly: new Hourly(null, temperature_2m: new List<double> { 10.9 }, null, null, null, null, null));

//            var weatherServiceHandlerMock = new Mock<IWeatherServiceHandler>();
//            weatherServiceHandlerMock.Setup(handler => handler.CallApi(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(weatherJson);

//            var positionServiceHandlerMock = new Mock<IPositionServiceHandler>();
//            positionServiceHandlerMock.Setup(handler => handler.CallApi(It.IsAny<string>())).ReturnsAsync(positionData);

//            var weatherDataTransformationsMock = new Mock<IWeatherDataTransformations>();
//            weatherDataTransformationsMock.Setup(transformations => transformations.JsonToWeatherData(It.IsAny<string>())).Returns(weatherData);

//            var service = new WeatherService(
//                weatherServiceHandlerMock.Object,
//                positionServiceHandlerMock.Object,
//                weatherDataTransformationsMock.Object,
//                cacheMock.Object
//            );

//            // Act
//            var result = service.GetWeather("test_location");

//            // Assert
//            Assert.Equal(weatherData, result);
//            cacheMock.Verify(cache => cache.Set("test_location", weatherData, It.IsAny<TimeSpan>()), Times.Once);
//            weatherServiceHandlerMock.Verify(handler => handler.CallApi(It.IsAny<double>(), It.IsAny<double>()), Times.Once);
//            positionServiceHandlerMock.Verify(handler => handler.CallApi(It.IsAny<string>()), Times.Once);
//            weatherDataTransformationsMock.Verify(transformations => transformations.JsonToWeatherData(weatherJson), Times.Once);
//        }
//    }
//}
