namespace Weather_App.Models
{
    public record WeatherCode(
        List<Weather_conditions> weather_conditions
        );

    public record Weather_conditions(
        int code,
        string description,
        string image
        );
}
