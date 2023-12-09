if (args.Length != 2)
{
    Console.WriteLine("No puzzle input files specified");
    Environment.Exit(-1);
}

string part1PuzzleInputFile = args[0];
string part2PuzzleInputFile = args[1];

if (!File.Exists(part1PuzzleInputFile))
{
    Console.WriteLine($"Part 1 Puzzle input file '{part1PuzzleInputFile}' not found");
    Environment.Exit(-1);
}

if (!File.Exists(part2PuzzleInputFile))
{
    Console.WriteLine($"Part 2 Puzzle input file '{part2PuzzleInputFile}' not found");
    Environment.Exit(-1);
}

Part1(part1PuzzleInputFile);
Part2(part2PuzzleInputFile);

void Part1(string puzzleInputFile)
{
    List<long[]> readings = GetReportReadings(puzzleInputFile);

    long extrapolatedValuesSum = 0;
    
    foreach (long[] readingValues in readings)
    {
        extrapolatedValuesSum += NextReading(readingValues);
    }
    
    Console.WriteLine($"Part 1 - {extrapolatedValuesSum}");
}

void Part2(string puzzleInputFile)
{
    List<long[]> readings = GetReportReadings(puzzleInputFile);

    long extrapolatedValuesSum = 0;
    
    foreach (long[] readingValues in readings)
    {
        Array.Reverse(readingValues);
        extrapolatedValuesSum += NextReading(readingValues);
    }
    
    Console.WriteLine($"Part 2 - {extrapolatedValuesSum}");
}

long NextReading(long[] readings)
{
    if (readings.Length == 0 || readings.All(r => r == 0)) return 0;
    if (readings.Length == 1) return readings[0];

    long[] nextReadings = new long[readings.Length - 1];

    for (long reading = 0; reading < readings.Length - 1; reading++)
    {
        nextReadings[reading] = readings[reading + 1] - readings[reading];
    }

    return  readings[^1] + NextReading(nextReadings);
}

List<long[]> GetReportReadings(string puzzleInputFile)
{
    List<long[]> readings = new();

    foreach (string line in File.ReadLines(puzzleInputFile))
    {
        readings.Add(line.Split(' ').Select(long.Parse).ToArray());
    }

    return readings;
}