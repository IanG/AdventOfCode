Part1("part1puzzleinput.txt");
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