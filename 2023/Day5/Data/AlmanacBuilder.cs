namespace Day5.Data;

public class AlmanacBuilder
{
    private readonly List<string> _data;
    
    public AlmanacBuilder(string[] data)
    {
        _data = data.ToList();
    }

    public Almanac Build()
    {
        return new Almanac()
        {
            Seeds = GetSeeds(),
            SeedToSoilMap = GetConversionMap("seed-to-soil"),
            SoilToFertilizerMap = GetConversionMap("soil-to-fertilizer"),
            FertilizerToWaterMap = GetConversionMap("fertilizer-to-water"),
            WaterToLightMap = GetConversionMap("water-to-light"),
            LightToTemperatureMap = GetConversionMap("light-to-temperature"),
            TemperatureToHumidityMap = GetConversionMap("temperature-to-humidity"),
            HumidityToLocationMap = GetConversionMap("humidity-to-location")
        };
    }

    private List<Seed> GetSeeds()
    {
        return _data[0].Substring(7).Split(' ')
            .ToList().Select(x => new Seed() { Id = long.Parse(x) }).ToList();
    }

    private ConversionMap GetConversionMap(string name)
    {
        List<Range> ranges = new();
        
        int headingPos = _data.IndexOf(name + " map:") + 1;

        foreach (string line in _data.Skip(headingPos))
        {
            if (string.IsNullOrEmpty(line)) break;

            string[] values = line.Split(' ');

            long destinationStart = long.Parse(values[0]);
            long sourceStart = long.Parse(values[1]);
            long rangeLength = long.Parse(values[2]);

            ranges.Add(new Range(sourceStart, destinationStart, rangeLength));
        }

        return new ConversionMap { Ranges = ranges };
    }
}