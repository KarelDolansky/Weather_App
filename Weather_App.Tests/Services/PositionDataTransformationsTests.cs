//using System.ComponentModel;
//using System.Text.Json;
//using Weather_App.Services;

//namespace Weather_App.Tests.Services
//{
//    public class PositionDataTransformationsTests
//    {

//        [Fact]
//        public void StringToPositionData_ValidJson_ReturnsFirstPositionData()
//        {
//            PositionDataTransformations positionDataTransformations = new PositionDataTransformations();
//            // Arrange
//            string jsonString = "[{\"place_id\":117367229,\"licence\":\"Data © OpenStreetMap contributors, ODbL 1.0. http://osm.org/copyright\",\"osm_type\":\"relation\",\"osm_id\":439073,\"lat\":\"50.7702648\",\"lon\":\"15.0583947\",\"class\":\"boundary\",\"type\":\"administrative\",\"place_rank\":16,\"importance\":0.583349587062047,\"addresstype\":\"city\",\"name\":\"Liberec\",\"display_name\":\"Liberec, okres Liberec, Liberecký kraj, Severovýchod, Česko\",\"boundingbox\":[\"50.7079791\",\"50.8243417\",\"14.9529697\",\"15.1468816\"]}]";
//            // Act
//            PositionData positionData = positionDataTransformations.StringToPositionData(jsonString);

//            // Assert
//            Assert.NotNull(positionData);
//            Assert.Equal(50.7702648, positionData.results[0].latitude);
//            Assert.Equal(15.0583947, positionData.results[0].longitude);
//            Assert.Equal("Liberec, okres Liberec, Liberecký kraj, Severovýchod, Česko", positionData.results[0].name);
//        }

//        [Fact]
//        public void StringToPositionData_EmptyJson_ThrowsException()
//        {

//            PositionDataTransformations positionDataTransformations = new PositionDataTransformations();
//            // Arrange
//            string jsonString = "";

//            // Assert
//            Assert.Throws<JsonException>(() => positionDataTransformations.StringToPositionData(jsonString));
//        }

//        [Fact]
//        public void StringToPositionData_InvalidJson_ThrowsException()
//        {

//            PositionDataTransformations positionDataTransformations = new PositionDataTransformations();
//            // Arrange
//            string jsonString = "[{invalid json}]";

//            // Assert
//            Assert.Throws<JsonException>(() => positionDataTransformations.StringToPositionData(jsonString));
//        }
//    }
//}
