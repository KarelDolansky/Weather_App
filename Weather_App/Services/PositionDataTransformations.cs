using System.Text.Json;

namespace Weather_App.Services
{
    public interface IPositionDataTransformations
    {
        PositionData StringToPositionData(string jsonString);
    }
    public class PositionDataTransformations : IPositionDataTransformations
    {
        public PositionData StringToPositionData(string jsonString)
        {
            PositionData data = JsonSerializer.Deserialize<PositionData>(jsonString);
            return data;
        }
    }
}
