public record PositionData(
    List<Results> results
    );
public record Results(
    string name,
    double latitude,
    double longitude
    );