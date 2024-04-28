namespace Weather_App.Services
{
    public interface IWeatherServiceHandler
    {
        public Task<WeatherData> CallApi(double latitude, double longitude);
    }

    public class WeatherServiceHandler: IWeatherServiceHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IWeatherDataTransformations _weatherDataTransformations;
        public WeatherServiceHandler(HttpClient httpClient,IWeatherDataTransformations weatherDataTransformations) 
        {
            _httpClient = httpClient;
            _weatherDataTransformations = weatherDataTransformations;
        }

        public async Task<WeatherData> CallApi(double latitude, double longitude)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,precipitation,rain,showers,snowfall,snow_depth&timezone=Europe%2FBerlin&forecast_days=1";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                
                return _weatherDataTransformations.StringToWeatherData(jsonString);
            }
            else
            {
                // Handle API call errors (e.g., log the error)
                throw new Exception($"API call failed with status code {response.StatusCode}");
            }
        }
    }
}
