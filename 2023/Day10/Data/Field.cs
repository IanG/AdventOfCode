using System.Text;

namespace Day10.Data;

public class Field
{
    private readonly char[,] _pipes;

    private readonly int _possibleDirections = 4;
    
    private readonly int[] _rowDirections = {-1, 0, 1, 0};
    private readonly int[] _columnDirections = {0, 1, 0, -1};
    
    private readonly Dictionary<(TileType tileType, Direction fromDirection), Direction> _pipeDirectionChanges = new()
    {
        { (TileType.Vertical, Direction.North), Direction.North },
        { (TileType.Vertical, Direction.South), Direction.South },
        { (TileType.Horizontal, Direction.East), Direction.West },
        { (TileType.Horizontal, Direction.West), Direction.East },
        { (TileType.NorthEastBend, Direction.West), Direction.North },
        { (TileType.NorthEastBend, Direction.South), Direction.East },
        { (TileType.NorthWestBend, Direction.South), Direction.East },
        { (TileType.NorthWestBend, Direction.West), Direction.North },
        { (TileType.SouthWestBend, Direction.South), Direction.West },
        { (TileType.SouthWestBend, Direction.East), Direction.South },
        { (TileType.SouthEastBend, Direction.East), Direction.South },
        { (TileType.SouthEastBend, Direction.South), Direction.East },
    };

    private readonly Dictionary<TileType, List<Direction>> _pipeEntryDirections = new()
    {
        { TileType.Vertical, [Direction.North, Direction.South] },
        { TileType.Horizontal, [Direction.East, Direction.West] },
        { TileType.NorthEastBend, [Direction.West, Direction.North] },
        { TileType.NorthWestBend, [Direction.East, Direction.South] },
        { TileType.SouthWestBend, [Direction.East, Direction.North] },
        { TileType.SouthEastBend, [Direction.West, Direction.South] }
    };

    public Field(char[,] pipes)
    {
        _pipes = pipes;
    }
    
    public TileType GetTileAt(Position position)
    {
        bool outsideNetwork = (position.Row < _pipes.GetLowerBound(0)) || (position.Row > _pipes.GetUpperBound(0)) ||
                              (position.Column < _pipes.GetLowerBound(1)) || (position.Column > _pipes.GetUpperBound(1));

        if (!outsideNetwork)
        {
            return (TileType)_pipes[position.Row, position.Column];    
        }

        return TileType.None;
    }

    public Direction GetTravelDirectionFrom(Position source, Position target)
    {
        return (source.Row - target.Row, source.Column - target.Column) switch {
            (0, 1) => Direction.East,
            (0, -1) => Direction.West,
            (1, 0) => Direction.North,
            (-1, 0) => Direction.South,
            _ => Direction.None
        };
    }

    public Direction GetExitDirectionFromPipe(TileType tileType, Direction direction)
    {
        if (_pipeDirectionChanges.ContainsKey((tileType, direction)))
        {
            return _pipeDirectionChanges[(tileType, direction)];
        }
        
        return Direction.None;
    }
    
    public List<Direction> GetEntryDirectionsToPipe(TileType tileType)
    {
        if (_pipeEntryDirections.ContainsKey(tileType))
        {
            return _pipeEntryDirections[tileType];
        }

        return new List<Direction>();
    }

    public List<Position> GetNeighboringPipes(Position position)
    {
        List<Position> connectedPoints = new();
        
        for(int i = 0; i < _possibleDirections; i++)
        {
            int newRow = position.Row + _rowDirections[i];
            int newColumn = position.Column + _columnDirections[i];

            if(newRow >= _pipes.GetLowerBound(0) && newRow < _pipes.GetUpperBound(0) && newColumn >= _pipes.GetLowerBound(1) && newColumn < _pipes.GetUpperBound(1))
            {
                Position p = new Position(newRow, newColumn);

                TileType tileType = GetTileAt(p);

                if (tileType != TileType.Ground && tileType != TileType.StartingPoint)
                {
                    connectedPoints.Add(p);    
                }
            }
        }
        
        return connectedPoints;
    }

    public Position? GetStartingPoint()
    {
        for (int row = 0; row <= _pipes.GetUpperBound(0); row++)
        {
            for (int column = 0; column <= _pipes.GetUpperBound(1); column++)
            {
                Position position = new Position(row, column);
                if(GetTileAt(position) == TileType.StartingPoint)
                {
                    return position;
                }
            }
        }

        return null;
    }

    public void Draw()
    {
        StringBuilder sb = new StringBuilder();
        
        for (int row = 0; row <= _pipes.GetUpperBound(0); row++)
        {
            for (int column = 0; column <= _pipes.GetUpperBound(1); column++)
            {
                char translatedChar = _pipes[row, column] switch
                {
                    '-' => '\u2550',
                    '|' => '\u2551',
                    'L' => '\u255a',
                    'J' => '\u255d',
                    '7' => '\u2557',
                    'F' => '\u2554',
                    '.' => ' ',
                    _ => _pipes[row, column]
                };

                sb.Append(translatedChar);
            }

            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }
}