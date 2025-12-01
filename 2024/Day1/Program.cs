using System.Text.RegularExpressions;

Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");

return 0;

void Part1(string filename)
{
    InputData inputData = GetInputData(filename);

    inputData.Left.Sort();
    inputData.Right.Sort();

    int totalDistance = inputData.Left.Zip(inputData.Right, (left, right) => Math.Abs(left - right)).Sum();
    
    Console.WriteLine($"Part 1: {totalDistance}");    
}

void Part2(string filename)
{
    InputData inputData = GetInputData(filename);

    Dictionary<int, int> rightFrequencies = inputData.Right.GroupBy(number => number)
        .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());
    
    int similarityScore = inputData.Left.Sum(l => rightFrequencies.TryGetValue(l, out int count) ? l * count : 0);
    
    Console.WriteLine($"Part 2: {similarityScore}");
}

InputData GetInputData(string filename)
{
    Regex splitRegex = new Regex(@"\s+", RegexOptions.Compiled);
    
    List<int> left = [];
    List<int> right = [];

    foreach (string line in File.ReadLines(filename))
    {
        string[] parts = splitRegex.Split(line);
        
        left.Add(int.Parse(parts[0]));
        right.Add(int.Parse(parts[1]));
    }

    return new InputData(left, right);
}

internal record InputData(List<int> Left, List<int> Right);