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
            Content = new StringContent("{\"results\":[{\"id\":3071961,\"name\":\"Liberec\",\"latitude\":50.76711,\"longitude\":15.05619,\"elevation\":359,\"feature_code\":\"PPLA\",\"country_code\":\"CZ\",\"admin1_id\":3339541,\"admin2_id\":3071960,\"admin3_id\":11924204,\"timezone\":\"Europe/Prague\",\"population\":97770,\"country_id\":3077311,\"country\":\"Czechia\",\"admin1\":\"Liberecký kraj\",\"admin2\":\"Liberec District\",\"admin3\":\"Liberec\"}],\"generationtime_ms\":1.0370016}")
        }));

        var mockPositionDataTransformations = new Mock<IPositionDataTransformations>();
        mockPositionDataTransformations.Setup(m => m.StringToPositionData(It.IsAny<string>()))
            .Returns(new PositionData(new List<Results> { new Results("Liberec",10,10) }));
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