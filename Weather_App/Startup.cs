using Weather_App.Services;


namespace Weather_App
{
    public class Startup
    {
        void ConfigureServices(IServiceCollection services)
        { 
            //services.AddHttpClient();
            //services.AddSingleton<IWeatherServiceHandler, WeatherServiceHandler>();
            services.AddSingleton<IWeatherService, WeatherService>();
        }

    }
}
