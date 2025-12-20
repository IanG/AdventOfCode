Part1("part1puzzleinput.txt");
void Part1(string filename)
{
    Safe safe = new(50);

    foreach (Rotation rotation in GetPart1Data(filename))
    {
        safe.Move(rotation);
    }
    
    Console.WriteLine($"Final Position: {safe.CurrentPosition}");
    Console.WriteLine($"Lands on zero: {safe.LandsOnZero}");
}

IEnumerable<Rotation> GetPart1Data(string filename)
{
    foreach (string line in File.ReadLines(filename))
    {
        char direction = line[0];
        int clicks = int.Parse(line.AsSpan(1));

        yield return new() { Direction = direction == 'L' ? Direction.Left : Direction.Right, Distance = clicks };
    }
}

internal sealed class Safe
{
    private const int MinPosition = 0;
    private const int MaxPosition = 99;
    private const int Range = MaxPosition - MinPosition + 1;

    public Safe(int startPosition = 0)
    {
        CurrentPosition = startPosition;
    }

    public int CurrentPosition { get; private set; }
    public int LandsOnZero { get; private set; }

    public void Move(Rotation rotation)
    {
        int clicks = rotation.Direction == Direction.Left ? -rotation.Distance : rotation.Distance;

        Rotate(clicks);
        
        if (CurrentPosition == 0) LandsOnZero++;
    }
    
    private void Rotate(int clicks)
    {
        CurrentPosition = ((CurrentPosition + clicks) % Range + Range) % Range;
    }
}

internal enum Direction
{
    Left = 1,
    Right = 2
}

internal readonly struct Rotation
{
    public Direction Direction { get; init; }
    public int Distance { get; init; }
}