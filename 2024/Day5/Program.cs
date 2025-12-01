
Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");

return;

void Part1(string filename)
{
    List<(int page, int before)> pageOrderingRules  = GetPageOrderingRules(filename);
    List<int[]> pageUpdates = GetPageUpdates(filename);

    foreach (int[] pages in pageUpdates)
    {
        bool isValidUpdate = true;

        for (int i = 0; i < pages.Length - 1; i++)
        {
            
        }
    }
    
    Console.WriteLine("Part 1: ?");
}

void Part2(string inputFileName)
{
    Console.WriteLine("Part 2: ?");
}

List<int[]> GetPageUpdates(string filename)
{
    List<int[]> pageUpdates = [];

    bool capture = false;
    foreach (string line in File.ReadLines(filename))
    {
        if (!capture)
        {
            if (string.IsNullOrWhiteSpace(line)) capture = true;
            continue;
        }

        pageUpdates.Add(line
            .Split(',')
            .Select(s => int.TryParse(s, out int n) ? n : 0)
            .ToArray());
    }
    
    return pageUpdates;
}

List<(int page, int before)> GetPageOrderingRules(string filename)
{
    List<(int page, int before)> pageOrderingRules = [];

    foreach (string line in File.ReadLines(filename))
    {
        if (string.IsNullOrWhiteSpace(line)) break;
        
        string[] parts = line.Split("|");
        
        pageOrderingRules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
    }
    
    return pageOrderingRules;
}