namespace Day2.Data;

public class GameBuilder
{
    private const int GameNumberStartPos = 5;
    private const char GameDelimiter = ':';
    private const char RevealDelimiter = ';';

    private readonly string _data;
    
    public GameBuilder(string data)
    {
        _data = data;
    }

    public Game Build()
    {
        return new Game { Id = GetGameId(), Reveals = GetReveals() };
    }

    private int GetGameId()
    {
        return int.Parse(_data.Substring(GameNumberStartPos,_data.IndexOf(GameDelimiter) - GameNumberStartPos));
    }

    private List<Reveal> GetReveals()
    {
        List<Reveal> reveals = new List<Reveal>();

        string revealsData = _data[(_data.IndexOf(GameDelimiter) + 1)..];

        foreach (string revealData in revealsData.Split(RevealDelimiter))
        {
            reveals.Add(new RevealBuilder(revealData).Build());
        }

        return reveals;
    }
}