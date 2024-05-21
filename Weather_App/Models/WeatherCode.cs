namespace Weather_App.Models
{
    public record WeatherCode(
        List<WeatherConditions> weather_conditions
        );

    public record WeatherConditions(
        int code,
        string description,
        string image
        );
}
