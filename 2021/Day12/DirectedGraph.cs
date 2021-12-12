using System.Collections.Generic;
using System.Linq;

namespace Day12
{
    public class DirectedGraph
    {
        private List<GraphNode> nodes = new List<GraphNode>();

        public DirectedGraph() {}

        public int Count
        {
            get { return nodes.Count; }
        }

        public IList<GraphNode> Nodes
        {
            get { return nodes.AsReadOnly(); }
        }

        public bool AddNode(GraphNode node)
        {
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
                return true;
            }

            return false;
        }        

        public bool AddNeighbours(GraphNode neighbour1, GraphNode neighbour2)
        {
            GraphNode node1 = FindGraphNode(neighbour1);
            GraphNode node2 = FindGraphNode(neighbour2);

            if (node1 == null || node2 == null)
            {
                return true;
            }
            else if (node1.Neighbours.Contains(node2))
            {
                return false;
            }
            else
            {
                if (!(neighbour2 is StartNode))
                {
                    node1.AddNeighbour(node2);
                }
                return true;
            }
        }

        private GraphNode FindGraphNode(GraphNode graphNode)
        {
            return nodes.First(node => node.Name.Equals(graphNode.Name));
        }

        public bool RemoveNode(GraphNode nodeToRemove)
        {
            GraphNode removeNode = FindGraphNode(nodeToRemove);

            if (removeNode != null)
            {
                nodes.Remove(removeNode);
                foreach (GraphNode node in nodes)
                {
                    node.RemoveNeighbour(removeNode);
                }
                return true;
            }

            return false;
        }

        public bool RemoveNeighbour(GraphNode neighbour1, GraphNode neighbour2)
        {
            GraphNode node1 = FindGraphNode(neighbour1);
            GraphNode node2 = FindGraphNode(neighbour2);

            if (node1 == null || node2 == null)
            {
                return false;
            }
            else if (!node1.Neighbours.Contains(node2))
            {
                return false;
            }
            else
            {
                node1.RemoveNeighbour(node2);
                return true;
            }
        }

        public void Clear()
        {
            foreach (GraphNode node in nodes)
            {
                node.RemoveAllNeighbours();
            }

            nodes.Clear();
        }

        public override string ToString()
        {
            if (Nodes.Count > 0 )
            {
                return $"Nodes={{{string.Join(", ", Nodes.Select(node => node.Name))}}}";
            }
            else
            {
                return "Nodes=None";
            }
        }
    }
}