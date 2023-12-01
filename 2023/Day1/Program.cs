using System.Text;

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
    int calibrationValueSum = 0;

    foreach (string line in File.ReadLines(puzzleInputFile))
    {
        int[] digits = GetDigits(line);
        int calibrationValue = CalibrationValueFromDigits(digits);
        calibrationValueSum += calibrationValue;
    }
    
    Console.WriteLine($"Part 1 - Sum of calibration values: {calibrationValueSum}");
}

void Part2(string puzzleInputFile)
{
    int calibrationValueSum = 0;

    foreach (string line in File.ReadLines(puzzleInputFile))
    {
        int[] digits = GetDigits(TranscribeDigitWordsToDigits(line));
        int calibrationValue = CalibrationValueFromDigits(digits);
        calibrationValueSum += calibrationValue;
    }
    
    Console.WriteLine($"Part 2 - Sum of calibration values: {calibrationValueSum}");
}

static string TranscribeDigitWordsToDigits(string line)
{
    // Covers overlapping numbers in word form
    // e.g. sevenine, eightwo, fiveight
    // leaving the first and last letters intact for overlaps
    
    return new StringBuilder(line)
        .Replace("one", "o1e")
        .Replace("two", "t2o")
        .Replace("three", "t3e")
        .Replace("four", "f4r")
        .Replace("five", "f5e")
        .Replace("six", "s6x")
        .Replace("seven", "s7n")
        .Replace("eight", "e8t")
        .Replace("nine", "n9e")
        .ToString();
}

static int[] GetDigits(string line)
{
    return line.Where(char.IsDigit).Select(c => (int)char.GetNumericValue(c)).ToArray();
}

static int CalibrationValueFromDigits(int[] digits)
{
    return (digits[0] * 10) + digits[^1];
}