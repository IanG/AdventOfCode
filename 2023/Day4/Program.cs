using Day4.Data;

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
    List<ScratchCard> scratchCards = GetScratchCards(puzzleInputFile);
    
    int pointsTotal = 0;
    foreach (ScratchCard scratchCard in scratchCards)
    {
        if (scratchCard.PickedWinningNumbers.Any())
        {
            pointsTotal += (int)Math.Pow(2, scratchCard.PickedWinningNumbers.Count - 1);
        }
    }
    
    Console.WriteLine($"Part 1 - Total Points {pointsTotal}");
}

void Part2(string puzzleInputFile)
{
    List<ScratchCard> scratchCards = GetScratchCards(puzzleInputFile);
    int[] scratchCardCounts = scratchCards.Select(_ => 1).ToArray();
    
    for (int index = 0; index < scratchCards.Count; index++)
    {
        ScratchCard scratchCard = scratchCards[index];
        int currentCardCount = scratchCardCounts[index];

        for (int win = 0; win < scratchCard.PickedWinningNumbers.Count; win++)
        {
            scratchCardCounts[index + win + 1] += currentCardCount;
        }
    }
    
    Console.WriteLine($"Part 2 - Total Scratchcards {scratchCardCounts.Sum()}");
}

List<ScratchCard> GetScratchCards(string puzzleInputFile)
{
    List<ScratchCard> scratchCards = new();

    foreach (string line in File.ReadLines(puzzleInputFile))
    {
        scratchCards.Add(new ScratchCardBuilder(line).Build());
    }

    return scratchCards;
}