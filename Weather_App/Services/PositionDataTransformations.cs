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
            List<PositionData> data = JsonSerializer.Deserialize<List<PositionData>>(jsonString);

            return data[0];
        }
    }
}
