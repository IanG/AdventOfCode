namespace Day4.Data;

public class ScratchCard
{
    public List<int> WinningNumbers { get; init; } = new();
    public List<int> PickedNumbers { get; init; } = new();
    public List<int> PickedWinningNumbers => WinningNumbers.Intersect(PickedNumbers).ToList();
}