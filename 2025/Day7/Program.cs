using System.Text;

const char start = 'S';
const char emptySpace = '.';
const char splitter = '^';
const char beam = '|';

Part1("part1puzzleinput.txt");

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

static char[,] GetTachyonManifoldDiagram(string filename)
{
    string[] lines = File.ReadAllLines(filename);
    
    int rows = lines.Length;
    int cols = lines[0].Length;
    
    char[,] diagram = new char[rows, cols];

    for (int row = 0; row < rows; row++)
    {
        string line = lines[row];

        for (int column = 0; column < cols; column++)
        {
            diagram[row, column] = line[column];
        }
    }
    
    return diagram;
} 