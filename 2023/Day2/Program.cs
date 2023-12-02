using Day2.Data;

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
    const int maxRedCubes = 12;
    const int maxGreenCubes = 13;
    const int maxBlueCubes = 14;
    
    int sumOfGameIds = 0;
    
    foreach (string line in File.ReadLines(puzzleInputFile))
    {
        Game game = new GameBuilder(line).Build();

        if (!game.Reveals.Exists(r =>
            r.RedCubes > maxRedCubes || r.GreenCubes > maxGreenCubes || r.BlueCubes > maxBlueCubes))
        {
            sumOfGameIds += game.Id;
        }
    }

    Console.WriteLine($"Part 1 - Sum of game ids: {sumOfGameIds}");
}

void Part2(string puzzleInputFile)
{
    int sumOfPowers = 0;
    
    foreach (string line in File.ReadLines(puzzleInputFile))
    {
        Game game = new GameBuilder(line).Build();

        int minRedCubes = game.Reveals.Max(r => r.RedCubes);
        int minGreenCubes = game.Reveals.Max(g => g.GreenCubes);
        int minBlueCubes = game.Reveals.Max(b => b.BlueCubes);

        int cubePower = minRedCubes * minGreenCubes * minBlueCubes;

        sumOfPowers += cubePower;
    }
    
    Console.WriteLine($"Part 2 - Sum of powers: {sumOfPowers}");
}