using System.Text.Json;

namespace Weather_App.Services
{
    public interface IPositionDataTransformations
    {
        PositionData JsonToPositionData(string jsonString);
    }
    public class PositionDataTransformations : IPositionDataTransformations
    {
        public PositionData JsonToPositionData(string jsonString)
        {
            PositionData data = JsonSerializer.Deserialize<PositionData>(jsonString);
            return data;
        }
    }
}
