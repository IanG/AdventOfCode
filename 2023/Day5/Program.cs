using Day5.Data;

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

Almanac almanac;

Part1(part1PuzzleInputFile);
Part2(part2PuzzleInputFile);

void Part1(string puzzleInputFile)
{
    almanac = new AlmanacBuilder(File.ReadAllLines(puzzleInputFile)).Build();

    long lowestLocationId = long.MaxValue;
    
    foreach (Seed seed in almanac.Seeds)
    {
        lowestLocationId = Math.Min(lowestLocationId, GetSeedLocation(seed.Id));
    }
    
    Console.WriteLine($"Part 1 - {lowestLocationId}");
}

void Part2(string puzzleInputFile)
{
    Console.WriteLine("Part 2");
    
    almanac = new AlmanacBuilder(File.ReadAllLines(puzzleInputFile)).Build();
    
    List<long> locations = new List<long>();

    foreach ((long start, long end) range in GetSeedRanges(almanac.Seeds))
    {
        locations.Add(GetLowestLocation(range.start, range.end));
    }

    Console.WriteLine($"Part 2 - {locations.Min()}");
}

long GetLowestLocation(long start, long end)
{
    Console.WriteLine($"Starting Start {start}, End {end}");
    
    long lowest = long.MaxValue;
    Object locker = new object();

    Parallel.For(start, end, seed =>
    {
        long location = GetSeedLocation(seed);
        lock (locker) { lowest = Math.Min(lowest, location); }
    });

    Console.WriteLine($"Finished {start}, End {end}, Lowest {lowest}");

    return lowest;
}

long GetSeedLocation(long seedId)
{
    long soil = almanac.SeedToSoilMap.Convert(seedId);
    long fertilizer = almanac.SoilToFertilizerMap.Convert(soil);
    long water = almanac.FertilizerToWaterMap.Convert(fertilizer);
    long light = almanac.WaterToLightMap.Convert(water);
    long temperature = almanac.LightToTemperatureMap.Convert(light);
    long humidity = almanac.TemperatureToHumidityMap.Convert(temperature);
    return almanac.HumidityToLocationMap.Convert(humidity);
}

List<(long start, long end)> GetSeedRanges(IEnumerable<Seed> seeds)
{
    List<(long start, long end)> seedRanges = new();

    foreach (IEnumerable<Seed> pair in seeds.Chunk(2))
    {
        Seed[] seed = pair.ToArray();
        seedRanges.Add((seed[0].Id, seed[0].Id + seed[1].Id - 1));
    }

    return seedRanges;
}