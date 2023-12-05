namespace Day5.Data;

public class Almanac
{
    public List<Seed> Seeds { get; init; } = new();
    public ConversionMap SeedToSoilMap { get; init; } = new();
    public ConversionMap SoilToFertilizerMap { get; init; } = new();
    public ConversionMap FertilizerToWaterMap { get; init; } = new();
    public ConversionMap WaterToLightMap { get; init; } = new();
    public ConversionMap LightToTemperatureMap { get; init; } = new();
    public ConversionMap TemperatureToHumidityMap { get; init; } = new();
    public ConversionMap HumidityToLocationMap { get; init; } = new();
}