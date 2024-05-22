using System.Text.Json;
using Weather_App.Services;

namespace Weather_App.Tests.Services
{
    public class PositionDataTransformationsTests
    {

        [Fact]
        public void StringToPositionData_ValidJson_ReturnsFirstPositionData()
        {
            PositionDataTransformations positionDataTransformations = new PositionDataTransformations();
            // Arrange
            string jsonString = "{\"results\":[{\"id\":3071961,\"name\":\"Liberec\",\"latitude\":50.76711,\"longitude\":15.05619,\"elevation\":359.0,\"feature_code\":\"PPLA\",\"country_code\":\"CZ\",\"admin1_id\":3339541,\"admin2_id\":3071960,\"admin3_id\":11924204,\"timezone\":\"Europe/Prague\",\"population\":97770,\"country_id\":3077311,\"country\":\"Czechia\",\"admin1\":\"Liberecký kraj\",\"admin2\":\"Liberec District\",\"admin3\":\"Liberec\"}],\"generationtime_ms\":1.0830164}";
            // Act
            PositionData positionData = positionDataTransformations.JsonToPositionData(jsonString);

            // Assert
            Assert.NotNull(positionData);
            Assert.Equal(50.76711, positionData.results[0].latitude);
            Assert.Equal(15.05619, positionData.results[0].longitude);
            Assert.Equal("Liberec", positionData.results[0].name);
        }

        [Fact]
        public void StringToPositionData_EmptyJson_ThrowsException()
        {

            PositionDataTransformations positionDataTransformations = new PositionDataTransformations();
            // Arrange
            string jsonString = "";

            // Assert
            Assert.Throws<JsonException>(() => positionDataTransformations.JsonToPositionData(jsonString));
        }

        [Fact]
        public void StringToPositionData_InvalidJson_ThrowsException()
        {

            PositionDataTransformations positionDataTransformations = new PositionDataTransformations();
            // Arrange
            string jsonString = "[{invalid json}]";

            // Assert
            Assert.Throws<JsonException>(() => positionDataTransformations.JsonToPositionData(jsonString));
        }
    }
}
