// See https://aka.ms/new-console-template for more information

Part1("part1puzzleinput.txt");

void Part1(string filename)
{
    List<long> invalidIds = [];
 
    foreach (Range range in GetRangesFromFile(filename))
    {
        for (long productId = range.Start; productId <= range.End; productId++)
        {
            if (IsInvalidProductId(productId)) invalidIds.Add(productId);
        }
    }
    
    Console.WriteLine($"Part 1: {invalidIds.Sum()}");
}

static bool IsInvalidProductId(long productId)
{
    const int maxLongDigits = 20;
    Span<char> buffer = stackalloc char[maxLongDigits];
    
    if (!productId.TryFormat(buffer, out int charsWritten)) return false;
    if (charsWritten % 2 != 0) return false;

    ReadOnlySpan<char> span = buffer[..charsWritten];
    int midPoint = span.Length / 2;

    return span[..midPoint].SequenceEqual(span[midPoint..]);
}

static IEnumerable<Range> GetRangesFromFile(string filename)
{
    const char rangeSeparator = ',';
    using StreamReader reader = new(filename);
    
    char[] buffer = new char[128];
    int bufferIndex = 0;

    int currentChar;
    while ((currentChar = reader.Read()) != -1)
    {
        char c = (char)currentChar;
        if (c == rangeSeparator)
        {
            if (bufferIndex <= 0) continue;
            
            yield return MakeRange(buffer.AsSpan(0, bufferIndex));
            bufferIndex = 0;
        }
        else
        {
            if (bufferIndex < buffer.Length)
            {
                buffer[bufferIndex++] = c;
            }
        }
    }
    
    if (bufferIndex > 0)
    {
        yield return MakeRange(buffer.AsSpan(0, bufferIndex));
    }
}

static Range MakeRange(ReadOnlySpan<char> span)
{
    int separatorIndex = span.IndexOf('-');

    if (separatorIndex == -1) 
        throw new ArgumentException("Invalid range format");

    return new Range 
    { 
        Start = long.Parse(span[..separatorIndex]), 
        End = long.Parse(span[(separatorIndex + 1)..]) 
    };
}

readonly struct Range
{
    public required long Start { get; init; }
    public required long End { get; init; }
}