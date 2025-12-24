const char start = 'S';
const char emptySpace = '.';
const char splitter = '^';
const char beam = '|';

Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");
static void Part1(string filename)
{
    int beamSplits = 0;
    
    char[,] diagram = GetTachyonManifoldDiagram(filename);
    
    int rows = diagram.GetLength(0);
    int columns = diagram.GetLength(1);
    
    for (int row = 0; row < diagram.GetLength(0); row++)
    {
        for (int column = 0; column < diagram.GetLength(1); column++)
        {
            int previousRow = row - 1;
            int nextRow = row + 1;
            
            switch (diagram[row, column])
            {
                case start:
                    if (nextRow < rows)
                    {
                        diagram[row + 1, column] = beam;    
                    }
                    break;
                case splitter:
                    if (previousRow >= 0 && diagram[previousRow, column] == beam)
                    {
                        beamSplits++;
                    
                        if (column - 1 >= 0) diagram[row, column - 1] = beam;
                        if (column + 1 < columns) diagram[row, column + 1] = beam;
                    }
                    break;
                
                case emptySpace:
                    if (previousRow >= 0 && diagram[previousRow, column] == beam)
                    {
                        diagram[row, column] = beam;
                    }
                    
                    break;
            }
        }
    }
    
    Console.WriteLine($"Part 1: {beamSplits}");
}

static void Part2(string filename)
{
    char[,] diagram = GetTachyonManifoldDiagram(filename);
    
    Dictionary<(int, int), long> memo = new();
    int startColumn = FindStartLocation(diagram);
    
    long totalPaths = CountUniquePaths(diagram, 1, startColumn, memo);
    
    Console.WriteLine($"Part 2: {totalPaths}");
}

static long CountUniquePaths(char[,] diagram, int row, int column, Dictionary<(int, int), long> memo)
{
    int rows = diagram.GetLength(0);
    int columns = diagram.GetLength(1);
    
    if (row >= rows) return 1;
    if (column < 0 || column >= columns) return 0;
    
    if (memo.TryGetValue((row, column), out long cachedResult)) return cachedResult;

    long result = 0;
    char currentCell = diagram[row, column];

    switch (currentCell)
    {
        case splitter:
            result += CountUniquePaths(diagram, row + 2, column - 1, memo);
            result += CountUniquePaths(diagram, row + 2, column + 1, memo);
            break;
        
        default:
            result = CountUniquePaths(diagram, row + 1, column, memo);
            break;
    }

    memo[(row, column)] = result;
    return result;
}

static int FindStartLocation(char[,] diagram)
{
    for (int column = 0; column < diagram.GetLength(1); column++)
    {
        if (diagram[0, column] == start)
        {
            return column;
        }
    }
    
    return -1;
}

static char[,] GetTachyonManifoldDiagram(string filename)
{
    string[] lines = File.ReadAllLines(filename);
    
    int rows = lines.Length;
    int columns = lines[0].Length;
    
    char[,] diagram = new char[rows, columns];

    for (int row = 0; row < rows; row++)
    {
        string line = lines[row];

        for (int column = 0; column < columns; column++)
        {
            diagram[row, column] = line[column];
        }
    }
    
    return diagram;
} 