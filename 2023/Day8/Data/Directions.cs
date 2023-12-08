namespace Day8.Data;

public class Directions
{
    private int _index;
    private readonly char[] _directions;
    
    public Directions(char[] directions)
    {
        _directions = directions;
    }
    
    public char Next
    {
        get
        {
            if (_index >= _directions.Length) _index = 0;
            return _directions[_index++];
        }
    }    
}