public record WeatherData(
    //double latitude,
    //double longitude,
    //double generationtime_ms,
    //int utc_offset_seconds,
    //string timezone,
    //string timezone_abbreviation,
    //int elevation,
    //HourlyUnits hourly_units,
    Hourly hourly
);

//public record HourlyUnits(
//    //string time,
//    //string temperature_2m,
//    //string precipitation,
//    //string rain,
//    //string showers,
//    //string snowfall,
//    //string snow_depth
//);

public record Hourly(
    List<string> time,
    List<double> temperature_2m,
    List<double> precipitation,
    List<double> rain,
    List<double> showers,
    List<double> snowfall,
    List<double> snow_depth
);