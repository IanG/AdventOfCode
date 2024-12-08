using System.Text.RegularExpressions;

Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");

return;

void Part1(string filename)
{
    string input = File.ReadAllText(filename);
    int sum = 0;
    
    Regex multiplyRegex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled);
    
    MatchCollection multiplications = multiplyRegex.Matches(input);

    foreach (Match multiplication in multiplications)
    {
        int x = int.Parse(multiplication.Groups[1].Value);
        int y = int.Parse(multiplication.Groups[2].Value);
        
        sum += x * y;
    }

    Console.WriteLine($"Part 1: {sum}");
}

void Part2(string filename)
{
    string input = File.ReadAllText(filename);
    bool shouldProcess = true;
    int sum = 0;

    Regex extendedMultiplicationRegex =
        new Regex(@"(?<do>do\(\))|(?<dont>don\'t\(\))|(?<mul>mul\((?<x>\d{1,3}),(?<y>\d{1,3})\))",
            RegexOptions.Compiled);
    
    MatchCollection extendedMultiplications = extendedMultiplicationRegex.Matches(input);

    foreach (Match match in extendedMultiplications)
    {
        if (match.Groups["do"].Success)
        {
            shouldProcess = true;
            continue;
        }

        if (match.Groups["dont"].Success)
        {
            shouldProcess = false;
            continue;
        }

        if (shouldProcess && match.Groups["mul"].Success)
        {
            int x = int.Parse(match.Groups["x"].Value);
            int y = int.Parse(match.Groups["y"].Value);

            sum += x * y;
        }
    }
    
    Console.WriteLine($"Part 2: {sum}");
}
