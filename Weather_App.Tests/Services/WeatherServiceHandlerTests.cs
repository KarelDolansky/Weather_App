using Moq.Protected;
using Moq;
using System.Net;
using System.Text;
using Weather_App.Services;

public class WeatherServiceHandlerTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly WeatherServiceHandler _weatherServiceHandler;

    public WeatherServiceHandlerTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _weatherServiceHandler = new WeatherServiceHandler(_httpClient);
    }

    [Fact]
    public async Task CallApi_ReturnsJsonString_WhenResponseIsSuccess()
    {
        // Arrange
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"latitude\":50.760002,\"longitude\":15.059999}", Encoding.UTF8, "application/json")
        };
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(fakeResponse);

        // Act
        var result = await _weatherServiceHandler.CallApi(50.760002, 15.059999, DateOnly.FromDateTime(DateTime.Now));

        // Assert
        Assert.Equal("{\"latitude\":50.760002,\"longitude\":15.059999}", result);
    }

    [Fact]
    public async Task CallApi_ThrowsExceptionApiCall_WhenResponseIsNotSuccess()
    {
        // Arrange
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(fakeResponse);

        // Act & Assert
        await Assert.ThrowsAsync<ExceptionApiCall>(() => _weatherServiceHandler.CallApi(50.760002, 15.059999, DateOnly.FromDateTime(DateTime.Now)));
    }
}

