public record WeatherData(
    double latitude,
    double longitude,
    Hourly hourly,
    Daily daily
);
public record Hourly(
    List<string> time,
    List<double> temperature_2m,
    List<double> precipitation,
    List<double> rain,
    List<double> showers,
    List<double> snowfall,
    List<double> snow_depth
);
public record Daily(
    List<int> weather_code
    );