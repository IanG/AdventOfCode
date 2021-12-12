using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectedGraph graph = DirectedGraphFromInputs(args[0]);

            List<List<GraphNode>> nodes = GetPathsThroughCaves(graph);
            Console.WriteLine($"Answer 1 {nodes.Count}");
        }

        public static List<List<GraphNode>> GetPathsThroughCaves(DirectedGraph caveMap)
        {
            List<List<GraphNode>> result = new List<List<GraphNode>>();

            GraphNode root = caveMap.Nodes.OfType<StartNode>().First();
            GraphNode goal = caveMap.Nodes.OfType<EndNode>().First();

            if (root != null && goal != null)
            {
                Queue<List<GraphNode>> queue = new Queue<List<GraphNode>>();
                queue.Enqueue(new List<GraphNode>() {root});

                while (queue.Count != 0)
                {
                    List<GraphNode> path = queue.Dequeue();
                    GraphNode lastNode = path.Last();

                    var smallCavesVisitedMoreThanOnce = path.OfType<SmallCaveNode>()
                        .GroupBy(node => node.Name)
                        .Select(n => new { 
                            MetricName = n.Key,
                            MetricCount = n.Count()
                        })
                        .Where(metric => metric.MetricCount > 1).Count();

                    if (smallCavesVisitedMoreThanOnce < 1)
                    {
                        if (lastNode == goal)
                        {
                            result.Add(path);
                        }
                        else 
                        {
                            foreach (GraphNode neighbour in lastNode.Neighbours)
                            {
                                List<GraphNode> nextPath = new List<GraphNode>();    
                                nextPath.AddRange(path);
                                nextPath.Add(neighbour);
                                queue.Enqueue(nextPath);
                            }
                        }
                    }
                } 
            }
            return result;
        }

        public static DirectedGraph DirectedGraphFromInputs(string inputFile)
        {
            DirectedGraph directedGraph = new DirectedGraph();

            foreach (GraphNode vertices in GetVertices(inputFile))
            {
                directedGraph.AddNode(vertices);
            }

            foreach (Tuple<GraphNode, GraphNode> adjacencent in GetAdjacencents(inputFile))
            {
                directedGraph.AddNeighbours(adjacencent.Item1, adjacencent.Item2);
            }

            return directedGraph;
        }

        private static List<GraphNode> GetVertices(string inputFile)
        {
            List<GraphNode> verticies = new List<GraphNode>();

            foreach (string line in System.IO.File.ReadLines(inputFile).ToList())
            {
                string[] nodeNames = line.Split("-", 2);
                foreach (string name in nodeNames)
                {                   
                    if(!verticies.Exists(node => node.Name.Equals(name)))
                    {
                        verticies.Add(MakeGraphNode(name));
                    }
                }
            }

            return verticies;
        }

        private static List<Tuple<GraphNode, GraphNode>> GetAdjacencents(string inputFile)
        {
            List<Tuple<GraphNode, GraphNode>> adjacencents = new List<Tuple<GraphNode, GraphNode>>();

            foreach (string line in System.IO.File.ReadLines(inputFile).ToList())
            {
                string[] adjacencentNames = line.Split("-", 2);
                
                adjacencents.Add(GetAdjacencent(adjacencentNames[0], adjacencentNames[1]));
                adjacencents.Add(GetAdjacencent(adjacencentNames[1], adjacencentNames[0]));
            }

            return adjacencents;
        }

        private static Tuple<GraphNode, GraphNode> GetAdjacencent(string name1, string name2)
        {
            return new Tuple<GraphNode, GraphNode>(MakeGraphNode(name1), MakeGraphNode(name2));
        }

        private static GraphNode MakeGraphNode(string name)
        {
            if ("start".Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return new StartNode();
            }
            else if ("end".Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return new EndNode();
            }
            else if (name.All(char.IsUpper))
            {
                return new LargeCaveNode(name);
            }
            else
            {
                return new SmallCaveNode(name);
            }
        }
    }
}
