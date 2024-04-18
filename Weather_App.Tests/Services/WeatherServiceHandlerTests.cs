using Moq;
using System.Net;
using Weather_App.Services;

namespace Weather_App.Tests
{
    public class WeatherServiceHandlerTests
    {
        [Fact]
        public async Task CallApi_Returns_WeatherData_On_Successful_Api_Call()
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"hourly\": { \"temperature_2m\": [10.9]}}")
            }));
            var mockWeatherDataTransformations = new Mock<IWeatherDataTransformations>();

            // Mock StringToWeatherData to return a valid WeatherData object
            var mockWeatherData = new Mock<WeatherData>();

            mockWeatherDataTransformations.Setup(x => x.StringToWeatherData(It.IsAny<string>())).Returns(new WeatherData(hourly: new Hourly(null, temperature_2m: new List<double> { 10.9 }, null, null, null, null, null)));

            var weatherServiceHandler = new WeatherServiceHandler(httpClient, mockWeatherDataTransformations.Object);
            var weatherData = await weatherServiceHandler.CallApi(50.0, 10.0);
            Assert.NotNull(weatherData);
        }


        [Fact]
        public async Task CallApi_Throws_Exception_On_Unsuccessful_Api_Call()
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            }));
            var mockWeatherDataTransformations = new Mock<IWeatherDataTransformations>();
            var weatherServiceHandler = new WeatherServiceHandler(httpClient, mockWeatherDataTransformations.Object);
            await Assert.ThrowsAsync<Exception>(() => weatherServiceHandler.CallApi(50.0, 10.0));
        }
    }

    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}
