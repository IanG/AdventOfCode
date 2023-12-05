namespace Day5.Data;

public class ConversionMap
{
    public List<Range> Ranges { get; init; } = new();

    public long Convert(long source)
    {
        Range? range = Ranges.FirstOrDefault(r => r.SourceStart <= source && r.SourceEnd >= source);

        if (range != default)
        {
            return range.DestinationStart + (source - range.SourceStart);
        }

        return source;
    }
}