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
    char[,] schematic = GetSchematic(puzzleInputFile);
    List<Symbol> symbols = GetSymbols(schematic);
    List<Number> numbers = GetNumbers(schematic);
    
    int sumOfPartNumbers = 0;
    
    foreach (Number number in numbers)
    {
        if (HasAdjacentSymbol(number, symbols))
        {
            sumOfPartNumbers += number.Value;
        }
    }

    Console.WriteLine($"Part 1 - Sum of part numbers {sumOfPartNumbers}");
}

void Part2(string puzzleInputFile)
{
    char[,] schematic = GetSchematic(puzzleInputFile);
    List<Symbol> symbols = GetSymbols(schematic).Where(s => s.Value == '*').ToList();
    List<Number> numbers = GetNumbers(schematic);

    int sumOfGearRatios = 0;
    foreach (Symbol symbol in symbols)
    {
        List<Number> adjacentNumbers = GetAdjacentNumbers(symbol, numbers);

        if (adjacentNumbers.Count == 2)
        {
            int gearRatio = adjacentNumbers.First().Value * adjacentNumbers.Last().Value;

            sumOfGearRatios += gearRatio;
        }
    }
    
    Console.WriteLine($"Part 1 - Sum of gear ratios {sumOfGearRatios}");
}

List<Number> GetAdjacentNumbers(Symbol symbol, List<Number> numbers)
{
    return numbers.Where(n => IsSymbolAdjacent(symbol, n)).ToList();
}

bool IsSymbolAdjacent(Symbol symbol, Number number)
{
    return GetNumberBoundaries(number).Any(b => b.Row == symbol.Row && b.Column == symbol.Column);
}

bool HasAdjacentSymbol(Number number, List<Symbol> symbols)
{
    List<Point> numberBoundaries = GetNumberBoundaries(number);
    
    foreach (Symbol symbol in symbols)
    {
        foreach (Point boundary in numberBoundaries)
        {
            if (symbol.Row == boundary.Row && symbol.Column == boundary.Column)
            {
                return true;
            }
        }
    }
    
    return false;
}

List<Point> GetNumberBoundaries(Number number)
{
    List<Point> boundaries = new List<Point>(15);

    int minRowIndex = number.Row - 1;
    int maxRowIndex = number.Row + 1;
    int minColumnIndex = number.Column - 1;
    int maxColumnIndex = number.Column + number.Length;
    
    for (int row = minRowIndex; row <= maxRowIndex; row++)
    {
        for (int column = minColumnIndex; column <= maxColumnIndex; column++)
        {
            boundaries.Add(new Point(row, column));
        }
    }
    
    boundaries.RemoveAll(p => p.Row == number.Row && p.Column >= number.Column && p.Column <= number.Column + number.Length - 1);
    
    return boundaries;
}

char[,] GetSchematic(string schematicInputFile)
{
    string[] lines = File.ReadAllLines(schematicInputFile);
    char[,] schematic = new char[lines[0].Length + 1, lines.Length + 1];

    for (int row = 0; row <= schematic.GetUpperBound(0); row++)
    {
        for (int column = 0; column <= schematic.GetUpperBound(1); column++)
        {
            if (row <= lines[0].Length - 1 && column <= lines.Length - 1)
            {
                schematic[row, column] = lines[row][column];
            }
            else
            {
                schematic[row, column] = '.';
            }
        }
    }

    return schematic;
}

List<Number> GetNumbers(char[,] schematic)
{
    List<Number> numbers = new List<Number>();
    List<char> numberChars = new();
    bool inNumber = false;
    int numberStartingColumn = 0;
    
    for (int row = 0; row <= schematic.GetUpperBound(0); row++)
    {
        for (int column = 0; column <= schematic.GetUpperBound(1); column++)
        {
            if (char.IsDigit(schematic[row, column]))
            {
                if (!inNumber)
                {
                    inNumber = true;
                    numberStartingColumn = column;
                }

                numberChars.Add(schematic[row, column]);
            }
            else
            {
                if (inNumber)
                {
                    numbers.Add(new Number(row, numberStartingColumn, DigitCharsToInt(numberChars.ToArray())));

                    inNumber = false;
                    numberChars.Clear();
                }
            }
        }
    }

    return numbers;
}

int DigitCharsToInt(char[] chars)
{
    int number = 0;
    
    foreach (char c in chars)
    {
        number = number * 10 + (c - '0');
    }

    return number;
}

List<Symbol> GetSymbols(char[,] schematic)
{
    List<Symbol> symbols = new();

    for (int row = 0; row <= schematic.GetUpperBound(0); row++)
    {
        for (int column = 0; column <= schematic.GetUpperBound(1); column++)
        {
            if (IsSymbol(schematic[row, column]))
            {
                symbols.Add(new Symbol(row, column, schematic[row, column]));
            }
        }
    }
    
    return symbols;
}

static bool IsSymbol(char c) => !char.IsDigit(c) && c != '.';

class Point
{
    public int Row { get; } 
    public int Column { get; }

    public Point(int row, int column) { Row = row; Column = column; }
}

sealed class Symbol : Point
{
    public char Value { get; }

    public Symbol(int row, int column, char value) : base(row, column) { Value = value; }
}

internal sealed class Number : Point
{
    public int Value { get; }
    public int Length => (int)Math.Floor(Math.Log10(Value) + 1);
    
    public Number(int row, int column, int value) : base(row, column) { Value = value; }
}