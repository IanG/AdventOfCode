
Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");

return;

void Part1(string filename)
{
    char[,] grid = GetGridInput(filename);

    int occurrences = CountWordOccurrences(grid, "XMAS");
    Console.WriteLine($"Part 1: {occurrences}");
}

void Part2(string filename)
{
    char[,] grid = GetGridInput(filename);
    
    int occurrences = CountXShapeOccurrences(grid);
    Console.WriteLine($"Part 2: {occurrences}");
}

int CountXShapeOccurrences(char[,] grid)
{
    List<string> matches = ["MAS", "SAM"];
    int n = grid.GetLength(0);
    int occurrences = 0;
    
    bool IsCentreOfMas(char c, int row, int column)
    {
        if (row - 1 < 0 || row + 1 >= n || column - 1 < 0 || column + 1 >= n) return false;
        
        char topLeft = grid[row - 1, column - 1];
        char topRight = grid[row - 1, column + 1];
        char bottomLeft = grid[row + 1, column - 1];
        char bottomRight = grid[row + 1, column + 1];

        string first = new String([topLeft, c, bottomRight]);
        string second = new String([bottomLeft, c, topRight]);

        return matches.Contains(first) && matches.Contains(second);
    }
    
    for (int row = 0; row < n; row++)
    {
        for (int col = 0; col < n; col++)
        {
            char c = grid[row, col];

            if (c != 'A') continue;
            if (IsCentreOfMas(c, row , col)) occurrences++;
        }
    }

    return occurrences;
}

int CountWordOccurrences(char[,] grid, string word)
{
    int n = grid.GetLength(0);
    int wordLength = word.Length;
    int occurrences = 0;
    
    int[,] directions = new int[,]
    {
        { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 },   // Horizontal & Vertical
        { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }  // Diagonals
    };
    
    // Check for word starting at grid[row, col]
    bool IsWordPresent(int row, int col, int dirRow, int dirCol)
    {
        for (int k = 0; k < wordLength; k++)
        {
            int newRow = row + k * dirRow;
            int newCol = col + k * dirCol;

            // Out of bounds ?
            if (newRow < 0 || newRow >= n || newCol < 0 || newCol >= n) return false;

            // Does this character match the next character of the word ?
            if (grid[newRow, newCol] != word[k]) return false;
        }
        return true;
    }
    
    for (int row = 0; row < n; row++)
    {
        for (int col = 0; col < n; col++)
        {
            // If the character matches the first character of the word
            if (grid[row, col] == word[0])
            {
                // Check all 8 possible directions
                for (int d = 0; d <= directions.GetUpperBound(0); d++)
                {
                    int dirRow = directions[d, 0];
                    int dirCol = directions[d, 1];

                    if (IsWordPresent(row, col, dirRow, dirCol)) occurrences++;
                }
            }
        }
    }

    return occurrences;
}

char[,] GetGridInput(string filename)
{
    List<string> inputs = System.IO.File.ReadLines(filename).ToList();

    char[,] grid = new char[inputs.Count, inputs.Count];

    int x = 0;
    foreach (string line in inputs)
    {
        int y = 0;
        foreach (char c in line)
        {
            grid[x, y] = c;
            y++;
        }
        x++;
    }
    
    return grid;
}