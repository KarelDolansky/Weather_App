using Moq;
using System.Net;
using System.Net.Http;
using Weather_App.Services;

public class PositionServiceHandlerTests
{
    [Fact]
    public async Task CallApi_ValidPosition_ReturnsPositionData()
    {
        // Arrange
        string position = "Prague, Czech Republic";
        var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"hourly\": { \"temperature_2m\": [10.9]}}")
        }));

        var mockPositionDataTransformations = new Mock<IPositionDataTransformations>();
        mockPositionDataTransformations.Setup(m => m.StringToPositionData(It.IsAny<string>()))
            .Returns(new PositionData ( "50.7702648", "15.0583947", "Liberec, okres Liberec, Liberecký kraj, Severovýchod, Česko" ));
        var positionServiceHandler = new PositionServiceHandler(httpClient, mockPositionDataTransformations.Object);
        var positionData = await positionServiceHandler.CallApi(position);
        Assert.NotNull(positionData);
    }

    [Fact]
    public async Task CallApi_ApiCallFails_ThrowsException()
    {
        // Arrange
        string position = "Prague, Czech Republic";
        var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        }));

        var mockPositionDataTransformations = new Mock<IPositionDataTransformations>();

        var positionServiceHandler = new PositionServiceHandler(httpClient, mockPositionDataTransformations.Object);
        await Assert.ThrowsAsync<Exception>(() => positionServiceHandler.CallApi(position));
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