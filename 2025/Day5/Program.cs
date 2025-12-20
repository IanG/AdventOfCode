Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");
static void Part1(string filename)
{
    CafeteriaData data = LoadCafeteriaData(filename);

    int validCount = 0;

    foreach (long id in data.AvailableIngredientIds)
    {
        foreach (FreshIngredientRange range in data.FreshIngredientRanges)
        {
            if (id >= range.Start && id <= range.End)
            {
                validCount++;
                break;
            }
        }
    }

    Console.WriteLine($"Part 1: {validCount}");
}

static void Part2(string filename)
{
    CafeteriaData data = LoadCafeteriaData(filename);
    FreshIngredientRange[] ranges = data.FreshIngredientRanges;

    if (ranges.Length == 0)
    {
        Console.WriteLine("Part 2: 0");
        return;
    }
    
    // Sort Ranges
    Array.Sort(ranges, (a, b) => a.Start.CompareTo(b.Start));
    
    // Merge/Reduce Ranges
    int mergedCount = 0; 

    for (int i = 1; i < ranges.Length; i++)
    {
        if (ranges[i].Start <= ranges[mergedCount].End + 1)
        {
            if (ranges[i].End > ranges[mergedCount].End)
            {
                ranges[mergedCount] = ranges[mergedCount] with { End = ranges[i].End };
            }
        }
        else
        {
            mergedCount++;
            ranges[mergedCount] = ranges[i];
        }
    }

    // Sum
    long totalUniqueIds = 0;
    ReadOnlySpan<FreshIngredientRange> mergedSpan = ranges.AsSpan(0, mergedCount + 1);
        
    foreach (FreshIngredientRange range in mergedSpan)
    {
        totalUniqueIds += (range.End - range.Start + 1);
    }

    Console.WriteLine($"Part 2: {totalUniqueIds}");
}

static CafeteriaData LoadCafeteriaData(string filename)
{
    List<FreshIngredientRange> ranges = new();
    List<long> productIds = new();
    bool parsingIds = false;

    foreach (string line in File.ReadLines(filename))
    {
        ReadOnlySpan<char> span = line;
        
        if (span.IsWhiteSpace())
        {
            parsingIds = true;
            continue;
        }

        if (!parsingIds)
        {
            int dashIndex = span.IndexOf('-');
            
            long start = long.Parse(span[..dashIndex]);
            long end = long.Parse(span[(dashIndex + 1)..]);
                
            ranges.Add(new FreshIngredientRange(start, end));
        }
        else
        {
            productIds.Add(long.Parse(span));
        }
    }

    return new CafeteriaData(ranges.ToArray(), productIds.ToArray());
}

readonly record struct FreshIngredientRange(long Start, long End);
readonly record struct CafeteriaData(FreshIngredientRange[] FreshIngredientRanges, long[] AvailableIngredientIds);