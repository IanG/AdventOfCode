namespace Day2.Data;

public class Game
{
    public int Id { get; init; }
    public List<Reveal> Reveals { get; init; } = new();
}