namespace Day8.Data;

public class Map
{
    public Directions Directions { get; }
    public List<MapNode> Nodes { get; }
    
    public Map(Directions directions, List<MapNode> nodes)
    {
        Directions = directions;
        Nodes = nodes;
    }
}