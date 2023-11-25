using System.Text.RegularExpressions;

const char OPEN_SPACE = '.';
const char WALL = '#';
const char OUT_OF_BOUNDS = ' ';


IEnumerable<string> input = System.IO.File.ReadLines(args[0]);

char[,] board = GetBoard(input);
Queue<string> instructions = GetInstructions(input);

Direction currentDirection = Direction.Right;
(int row, int column) currentPosition = GetStartingPosition(board);

PrintBoard(board, currentPosition, currentDirection);

while(instructions.Count > 0)
{
    string instruction = instructions.Dequeue();

    if (Int32.TryParse(instruction, out _))
    {
        int moves = Int32.Parse(instruction);

        switch(currentDirection)
        {
        case Direction.Up:
            currentPosition = MoveUp(moves);
            break;
        case Direction.Down:
            currentPosition = MoveDown(moves);
            break;
        case Direction.Left:
            currentPosition = MoveLeft(moves);
            break;
        case Direction.Right:
            currentPosition = MoveRight(moves);
            break;
        }
    }
    else
    {
        currentDirection = ChangeDirection(currentDirection, instruction);
        Console.WriteLine($"Direction is now {currentDirection}");
    }

    PrintBoard(board, currentPosition, currentDirection);
}

(int row, int column) MoveUp(int moves)
{
    return currentPosition;
}

(int row, int column) MoveDown(int moves)
{
    return currentPosition;
}

(int row, int column) MoveLeft(int moves)
{
    return currentPosition;
}

(int row, int column) MoveRight(int moves)
{
    int nextColumn = currentPosition.column;

    for(int move = 1; move <= moves; move++)
    {
        if (board[currentPosition.row, currentPosition.column + move] == WALL)
        {
            return currentPosition;
        }
        else if (board[currentPosition.row, currentPosition.column + move] == OUT_OF_BOUNDS)
        {
            nextColumn = currentPosition.column + (move - 1);
        }

    }
    return (currentPosition.row, nextColumn);
}


Direction ChangeDirection(Direction currentDirection, string instruction)
{
    const string TURN_LEFT = "L";
    const string TURN_RIGHT = "R";

    Dictionary<(Direction current, string instruction), Direction> directions = new ()
    {
        { ( Direction.Up, TURN_LEFT ), Direction.Left },
        { ( Direction.Up, TURN_RIGHT ), Direction.Right },
        { ( Direction.Down, TURN_LEFT ), Direction.Right },
        { ( Direction.Down, TURN_RIGHT ), Direction.Left },
        { ( Direction.Left, TURN_LEFT ), Direction.Down },
        { ( Direction.Left, TURN_RIGHT ), Direction.Up },
        { ( Direction.Right, TURN_LEFT ), Direction.Up },
        { ( Direction.Right, TURN_RIGHT ), Direction.Down }
    };

    return directions[(currentDirection, instruction)];
}

(int row, int column) GetStartingPosition(char[,] board)
{
    int row = 0;
    int column = 0;

    for(int col = 0; col <= board.GetUpperBound(1); col++)
    {
        if (board[row, col] == OPEN_SPACE)
        {
            column = col;
            break;
        }
    }
    
    return (row, column);
}

Queue<string> GetInstructions(IEnumerable<string> input)
{
    const string INSTRUCTION_REGEX = @"([LR]|\d*)";
    Regex regex = new Regex(INSTRUCTION_REGEX, RegexOptions.Singleline);
    Queue<String> instructions = new Queue<string>();
    
    regex.Matches(input.Last())
        .Select(instruction => instruction.Value)
        .Where(instruction => !String.IsNullOrEmpty(instruction))
        .ToList()
        .ForEach(instruction => instructions.Enqueue(instruction));

    return instructions;
}

char[,] GetBoard(IEnumerable<string> input)
{
    string[] boardLines = input.TakeWhile(line => !String.IsNullOrEmpty(line)).ToArray();
    char[,] board = new char[boardLines.Count(), boardLines.Max(line => line.Length)];

    for (int row = 0; row < boardLines.Length; row++)
    {
        for (int column = 0; column <= board.GetUpperBound(1); column++)
        {
            if (column < boardLines[row].Length)
                board[row, column] = boardLines[row][column];
            else
                board[row, column] = OUT_OF_BOUNDS;
        }
    }

    return board;
}

void PrintBoard(char[,] board, (int row, int column) currentPosition, Direction currentDirection)
{
    for (int row = 0; row <= board.GetUpperBound(0); row++)
    {
        for (int column = 0; column <= board.GetUpperBound(1); column++)
        {
            if ((row, column) == currentPosition)
            {
                char directionChar = '>';

                switch (currentDirection)
                {
                    case Direction.Right:
                        directionChar = '>';
                        break;
                    case Direction.Left:
                        directionChar = '>';
                        break;
                    case Direction.Up:
                        directionChar = '^';
                        break;
                    case Direction.Down:
                        directionChar = 'v';
                        break;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(directionChar);
                Console.ResetColor();
            }
            else
            {
                Console.Write(board[row, column]);
            }
            
        }
        Console.WriteLine();
    }
}

public enum Direction {
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3
}
