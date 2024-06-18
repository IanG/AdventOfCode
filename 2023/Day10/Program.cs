using System.Runtime.InteropServices.JavaScript;
using Day10.Data;

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
    Field field = GetField(puzzleInputFile);
    
    field.Draw();

    List<Position> visited = new();
    Position? startingPoint = field.GetStartingPoint(); ;

    if (startingPoint != null)
    {
        Position currentPosition = startingPoint;
        Direction currentDirection = Direction.None;
        
        while (true)
        {
            List<Position> neighboringPipes = field.GetNeighboringPipes(currentPosition)
                .Except(visited).ToList();

            if (!neighboringPipes.Contains(startingPoint))
            {
                Position nextPosition = null;

                foreach (Position neighboringPipe in neighboringPipes)
                {
                    TileType tileType = field.GetTileAt(neighboringPipe);
                    
                    Direction travelDirection = currentDirection == Direction.None ? 
                         field.GetTravelDirectionFrom(currentPosition, neighboringPipe) : currentDirection;
                    
                    if (field.GetEntryDirectionsToPipe(tileType).Contains(travelDirection))
                    {
                        Direction exitDirection = field.GetExitDirectionFromPipe(tileType, travelDirection);

                        if (exitDirection != Direction.None)
                        {
                            visited.Add(neighboringPipe);
                            currentPosition = neighboringPipe;
                            currentDirection = exitDirection;
                            break;
                        }    
                    }
                    
                    // Direction travelDirection = currentDirection == Direction.None ? 
                    //     field.GetTravelDirectionFrom(currentPosition, neighboringPipe) : currentDirection;
                
                    
                }
            
                if (neighboringPipes.Count == 0)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }
    
    
    
    Console.WriteLine($"Part 1 - {(visited.Count + 1) / 2}");
}

void Part2(string puzzleInputFile)
{
    Console.WriteLine($"Part 2 - ???");
}

Field GetField(string puzzleInputFile)
{
    string[] lines = File.ReadAllLines(puzzleInputFile);
    char[,] pipes = new char[lines.Length, lines[0].Length];
    
    for (int row = 0; row <= pipes.GetUpperBound(0); row++)
    {
        for (int column = 0; column <= pipes.GetUpperBound(1); column++)
        {
            pipes[row, column] = lines[row][column];
        }
    }
    
    return new Field(pipes);
}
