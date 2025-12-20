const char rollOfPaper = '@';

Part1("part1puzzleinput.txt");

static void Part1(string filename)
{
    char[,] grid = LoadGrid(filename);
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    int countWithFewerThan4 = 0;

    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < cols; x++)
        {
            if (grid[y, x] == rollOfPaper) 
            {
                if (CountNeighbors(grid, x, y, rows, cols) < 4)
                {
                    countWithFewerThan4++;
                }
            }
        }
    }

    Console.WriteLine($"Part 1: {countWithFewerThan4}");
}

static int CountNeighbors(char[,] grid, int x, int y, int rows, int cols)
{
    int count = 0;
    
    ReadOnlySpan<(int dy, int dx)> directions = [
        (-1, 0), (1, 0), (0, -1), (0, 1),
        (-1, -1), (-1, 1), (1, -1), (1, 1)
    ];

    foreach ((int dy, int dx) in directions)
    {
        int ny = y + dy;
        int nx = x + dx;
        
        if (ny >= 0 && ny < rows && nx >= 0 && nx < cols)
        {
            if (grid[ny, nx] == rollOfPaper)
            {
                count++;
            }
        }
    }

    return count;
}

static char[,] LoadGrid(string filename)
{
    string[] lines = File.ReadAllLines(filename); 
    int rows = lines.Length;
    int cols = lines[0].Length;

    char[,] grid = new char[rows, cols];

    for (int y = 0; y < rows; y++)
    {
        ReadOnlySpan<char> lineSpan = lines[y];
        for (int x = 0; x < cols; x++)
        {
            grid[y, x] = lineSpan[x];
        }
    }
    
    return grid;
}
