namespace Weather_App.Services
{
    public interface IWeatherServiceHandler
    {
        public Task<string> CallApi(double latitude, double longitude, int days = 1);
    }

    public class WeatherServiceHandler: IWeatherServiceHandler
    {
        private readonly HttpClient _httpClient;
        public WeatherServiceHandler(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<string> CallApi(double latitude, double longitude,int days=1)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,precipitation,rain,showers,snowfall,snow_depth&timezone=Europe%2FBerlin&forecast_days={days}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                return jsonString;
            }
            else
            {
                // Handle API call errors (e.g., log the error)
                throw new Exception($"API call failed with status code {response.StatusCode}");
            }
        }
    }
}
