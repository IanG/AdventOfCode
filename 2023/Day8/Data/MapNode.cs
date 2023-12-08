namespace Day8.Data;

public class MapNode
{
    public string Value { get; }
    public MapNode Left { get; set; }
    public MapNode Right { get; set;  }

    public MapNode(string value)
    {
        Value = value;
    }

    public override string ToString() => $"{Value}";
}