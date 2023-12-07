using Day7.Data;

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
    List<Hand> hands = GetHands(puzzleInputFile).OrderBy(x => x, Hand.ByStrengthAscending).ToList();
    
    long totalWinnings = hands.Select((hand, index) => hand.Bid * (index + 1)).Sum();
    
    Console.WriteLine($"Part 1 - {totalWinnings}");
}

void Part2(string puzzleInputFile)
{
    List<Hand> hands = GetHands(puzzleInputFile).OrderBy(x => x, Hand.ByStrengthAscendingWithJoker).ToList();
    
    long totalWinnings = hands.Select((hand, index) => hand.Bid * (index + 1)).Sum();
    
    Console.WriteLine($"Part 2 - {totalWinnings}");
}

List<Hand> GetHands(string puzzleInputFile)
{
    List<Hand> hands = new();
    
    foreach (string line in File.ReadLines(puzzleInputFile))
    {
        string[] arguments = line.Split(' ');

        hands.Add(new Hand(arguments[0].ToArray(), int.Parse(arguments[1])));
    }

    return hands;
}