namespace Weather_App.Services
{
    public interface IWeatherServiceHandler
    {
        public Task<string> CallApi(double latitude, double longitude, DateOnly date);
    }

    public class WeatherServiceHandler : IWeatherServiceHandler
    {
        private readonly HttpClient _httpClient;
        public WeatherServiceHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CallApi(double latitude, double longitude, DateOnly date)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,precipitation&daily=weather_code&timezone=Europe%2FBerlin&start_date={date.Year}-{date.Month:D2}-{date.Day:D2}&end_date={date.Year}-{date.Month:D2}-{date.Day:D2}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                return jsonString;
            }
            else
            {
                throw new ExceptionApiCall("Api call failed " + response.StatusCode);
            }
        }
    }
}
