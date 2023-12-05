namespace Day5.Data;

public class Range
{
    public long SourceStart { get; }
    public long SourceEnd { get; }
    public long DestinationStart { get; }
    public long RangeLength { get; }

    public Range(long sourceStart, long destinationStart, long rangeLength)
    {
        SourceStart = sourceStart;
        DestinationStart = destinationStart;
        RangeLength = rangeLength;
        SourceEnd = SourceStart + RangeLength - 1;
    }
}