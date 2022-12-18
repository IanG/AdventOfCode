IEnumerable<string> rucksacks = System.IO.File.ReadLines(args[0]);

int prioritySum = rucksacks.Select(rucksack => GetPriority(CommonInCompartments(rucksack))).Sum();

Console.WriteLine($"Part 1: {prioritySum}");

prioritySum = rucksacks.Chunk(3)
    .Select(groupRucksacks => groupRucksacks[0]
        .Intersect(groupRucksacks[1])
        .Intersect(groupRucksacks[2])
        .First())
    .Select(GetPriority)
    .Sum();

Console.WriteLine($"Part 2: {prioritySum}");

char CommonInCompartments(string rucksack)
{
    string[] compartments = new string[]
    {
        rucksack.Substring(0, rucksack.Length / 2),
        rucksack.Substring(rucksack.Length / 2)
    };

    return compartments[0].Intersect(compartments[1]).First();
}

int GetPriority(char item)
{
    if(Char.IsLower(item))
    {
        return (int)item - 96;
    }
    else
    {
        return (int)item - 38;
    }
}

