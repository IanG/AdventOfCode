using System.Text.RegularExpressions;
using Day8.Data;

if (args.Length != 2)
{
    Console.WriteLine("No puzzle input files specified");
    Environment.Exit(-1);
}

string part1PuzzleInputFile = args[0];
string part2PuzzleInputFile = args[1];

if (!File.Exists(part1PuzzleInputFile))
{
    Console.WriteLine($"Part 1 Puzzle input file '{part1PuzzleInputFile}' not found");
    Environment.Exit(-1);
}

if (!File.Exists(part2PuzzleInputFile))
{
    Console.WriteLine($"Part 2 Puzzle input file '{part2PuzzleInputFile}' not found");
    Environment.Exit(-1);
}

Part1(part1PuzzleInputFile);
Part2(part2PuzzleInputFile);

void Part1(string puzzleInputFile)
{
    Map map = GetMap(puzzleInputFile);
    
    int stepsTaken = 0;
    MapNode? currentNode = map.Nodes.FirstOrDefault(n => n.Value == "AAA");
    MapNode? endNode = map.Nodes.FirstOrDefault(n => n.Value == "ZZZ");

    while (currentNode != endNode)
    {
        currentNode = map.Directions.Next switch
        {
            'L' => currentNode?.Left,
            'R' => currentNode?.Right,
            _ => throw new Exception("No Path to take")
        };
        
        stepsTaken++;
    }

    Console.WriteLine($"Part 1 - {stepsTaken}");
}

void Part2(string puzzleInputFile)
{
    Map map = GetMap(puzzleInputFile);
    
    Dictionary<(MapNode startNode, MapNode endNode), long> journeys = new();
    
    foreach (MapNode startNode in map.Nodes.Where(n => n.Value.EndsWith('A')))
    {
        int stepsTaken = 0;
        MapNode currentNode = startNode;

        while (!currentNode.Value.EndsWith('Z'))
        {
            currentNode = map.Directions.Next switch
            {
                'L' => currentNode.Left,
                'R' => currentNode.Right,
                _ => throw new Exception("No Path to take")
            };

            stepsTaken++;
        }

        journeys.Add((startNode, currentNode), stepsTaken);
    }

    long totalStepsTaken = LcmFromList(journeys.Select(x => x.Value));
    Console.WriteLine($"Part 2 - {totalStepsTaken}");
}

static long Gcd(long a, long b)
{
    //  Euclid's Greatest Common Divisor 
    if (b == 0) return a;
    return Gcd(b, a % b);
}

static long Lcm(long a, long b)
{
    // Lowest Common Multiple
    return a * b / Gcd(a, b);
}

static long LcmFromList(IEnumerable<long> numbers)
{
    return numbers.Aggregate<long, long>(1, Lcm);
}

Map GetMap(string puzzleInputFile)
{
    string[] lines = File.ReadAllLines(puzzleInputFile);

    Directions directions = new Directions(lines[0].ToArray());

    Dictionary<string, (string leftNodeName, string rightNodeName)> dict =
        ExtractNodeConnectionsFromLines(lines.Skip(2));

    Dictionary<string, MapNode> mapNodes = dict.ToDictionary(k => k.Key, v => new MapNode(v.Key));

    foreach (KeyValuePair<string, MapNode> mapNode in mapNodes)
    {
        mapNode.Value.Left = mapNodes[dict[mapNode.Key].leftNodeName];
        mapNode.Value.Right = mapNodes[dict[mapNode.Key].rightNodeName];
    }

    return new Map(directions, mapNodes.Values.ToList());
}

Dictionary<string, (string leftNodeName, string rightNodeName)> ExtractNodeConnectionsFromLines(IEnumerable<string> lines)
{
    Regex regex = new Regex(@"^(?<node>[A-Z0-9]{3})\s+\=\s+\((?<left>[A-Z0-9]{3})\,\s+(?<right>[A-Z0-9]{3})\)$", RegexOptions.Compiled);
    Dictionary<string, (string, string)> nodeConnections = new Dictionary<string, (string, string)>();
    
    foreach (string line in lines)
    {
        Match match = regex.Match(line);
        string nodeName = match.Groups["node"].Value;
        string leftNodeName = match.Groups["left"].Value;
        string rightNodeName = match.Groups["right"].Value;

        nodeConnections.Add(nodeName, (leftNodeName, rightNodeName));
    }

    return nodeConnections;
}
