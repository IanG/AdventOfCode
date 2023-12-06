using System.Text.RegularExpressions;
using Day6.Data;

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
    List<Race> races = GetRaces(puzzleInputFile);
    List<long> waysToBeatRecord = races.Select(WaysToBeatDistance).ToList();

    Console.WriteLine($"Part 1 - {waysToBeatRecord.Aggregate(1L, (a, b) => a * b)}");
}

void Part2(string puzzleInputFile)
{
    Race race = GetRace(puzzleInputFile);
    
    Console.WriteLine($"Part 2 - {WaysToBeatDistance(race)}");
}

long WaysToBeatDistance(Race race)
{
    long waysToWin = 0;

    for (long buttonHoldMs = 0; buttonHoldMs < race.Time; buttonHoldMs++)
    {
        long distanceTravelled = buttonHoldMs * (race.Time - buttonHoldMs);
    
        if (distanceTravelled > race.Distance) waysToWin++;
    }
    
    return waysToWin;
}

Race GetRace(string puzzleInputFile)
{
    string[] lines = File.ReadAllLines(puzzleInputFile);

    return new Race { Time = GetLineValue(lines[0], ':'), Distance = GetLineValue(lines[1], ':') };
}

List<Race> GetRaces(string puzzleInputFile)
{
    string[] lines = File.ReadAllLines(puzzleInputFile);

    long[] times = GetLineValues(lines[0], ':');
    long[] distances = GetLineValues(lines[1], ':');

    return times.Select((t, i) => new Race { Time = t, Distance = distances[i] }).ToList();
}

long GetLineValue(string line, char delimiter)
{
    return long.Parse(line[(line.IndexOf(delimiter) + 1)..].Replace(" ", ""));
}

long[] GetLineValues(string line, char delimiter)
{
    return Regex.Split(line[(line.IndexOf(delimiter) + 1)..].Trim(), @"\s+")
        .Select(long.Parse)
        .ToArray();
}