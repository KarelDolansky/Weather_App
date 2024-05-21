using Moq.Protected;
using Moq;
using System.Net;
using System.Text;
using Weather_App.Services;

public class PositionServiceHandlerTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<IPositionDataTransformations> _mockPositionDataTransformations;
    private readonly PositionServiceHandler _positionServiceHandler;

    public PositionServiceHandlerTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockPositionDataTransformations = new Mock<IPositionDataTransformations>();
        _positionServiceHandler = new PositionServiceHandler(_httpClient, _mockPositionDataTransformations.Object);
    }

    [Fact]
    public async Task CallApi_ReturnsPositionData_WhenResponseIsSuccess()
    {
        // Arrange
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"results\":[{\"name\":\"Test\",\"latitude\":50.0,\"longitude\":14.0}]}", Encoding.UTF8, "application/json")
        };
        var fakePositionData = new PositionData(new List<Results> { new Results("Test", 50.0, 14.0) });
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(fakeResponse);
        _mockPositionDataTransformations.Setup(p => p.JsonToPositionData(It.IsAny<string>())).Returns(fakePositionData);

        // Act
        var result = await _positionServiceHandler.CallApi("Test");

        // Assert
        Assert.Equal(fakePositionData, result);
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
        await Assert.ThrowsAsync<ExceptionApiCall>(() => _positionServiceHandler.CallApi("Test"));
    }
}
