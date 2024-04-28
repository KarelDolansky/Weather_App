public record WeatherData(
    double latitude,
    double longitude,
    Hourly hourly
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