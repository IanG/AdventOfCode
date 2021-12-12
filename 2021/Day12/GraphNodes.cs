using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Day12
{
    public abstract class GraphNode 
    {
        public string Name { get; }
        private List<GraphNode> neighbours;

        public GraphNode(string name) 
        { 
            Name = name; 
            neighbours = new List<GraphNode>();
        }

        public IList<GraphNode> Neighbours 
        { 
            get { return neighbours.AsReadOnly(); }
        }

        public bool AddNeighbour(GraphNode neighbour)
        {
            if(neighbour != this && !neighbours.Contains(neighbour))
            {
                neighbours.Add(neighbour);
                return true;
            }
            return false;
        }

        public bool RemoveNeighbour(GraphNode neighbour)
        {
            return neighbours.Remove(neighbour);
        }

        public void RemoveAllNeighbours()
        {
            neighbours.Clear();
        }

        public override string ToString() 
        { 
            StringBuilder sb = new StringBuilder();

            sb.Append($"Name={Name} Type={this.GetType().Name} Neighbours=");
            if (neighbours.Count > 0)
            {
                sb.Append($"{{{string.Join(", ", neighbours.Select(neighbour => neighbour.Name))}}}");
            }
            else
            {
                sb.Append("None");
            }
            return sb.ToString();
        }
    } 

    public sealed class StartNode : GraphNode 
    {
        public StartNode() : base("start") {}
    }

    public sealed class EndNode : GraphNode 
    {
        public EndNode() : base("end") {}
    }

    public abstract class CaveNode : GraphNode 
    {
        public CaveNode(string name) : base(name) {}
    }

    public sealed class SmallCaveNode : CaveNode 
    {
        public SmallCaveNode(string name) : base(name) {}
    }

    public sealed class LargeCaveNode : CaveNode 
    {
        public LargeCaveNode(string name) : base(name) {}
    }
}