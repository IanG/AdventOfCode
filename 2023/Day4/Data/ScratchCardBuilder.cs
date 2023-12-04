namespace Day4.Data;

public class ScratchCardBuilder
{
    private const char ScratchCardIdDelimiter = ':';
    private const char WinningNumbersDelimiter = '|';
    
    private readonly string _data;

    public ScratchCardBuilder(string data)
    {
        _data = data;
    }

    public ScratchCard Build()
    {
        return new ScratchCard { WinningNumbers = GetWinningNumbers(), PickedNumbers = GetPickedNumbers()};
    }
    
    private List<int> GetWinningNumbers()
    {
        List<int> winningNumbers = new();
        
        string winningNumbersData = _data[(_data.IndexOf(ScratchCardIdDelimiter) + 2)..(_data.IndexOf(WinningNumbersDelimiter) -1)];

        foreach (string winningNumber in winningNumbersData.Split(' '))
        {
            if (!string.IsNullOrEmpty(winningNumber))
            {
                winningNumbers.Add(int.Parse(winningNumber));
            }
        }
        
        return winningNumbers;
    }

    private List<int> GetPickedNumbers()
    {
        List<int> pickedNumbers = new();

        string pickedNumbersData = _data[(_data.IndexOf(WinningNumbersDelimiter) + 2)..];

        foreach (string pickedNumber in pickedNumbersData.Split(' '))
        {
            if (!string.IsNullOrEmpty(pickedNumber))
            {
                pickedNumbers.Add(int.Parse(pickedNumber));
            }
        }

        return pickedNumbers;
    }
}