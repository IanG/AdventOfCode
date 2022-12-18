IEnumerable<string> rucksacks = System.IO.File.ReadLines(args[0]);

int prioritySum = 0;

foreach(string rucksack in rucksacks)
{
    Console.WriteLine(rucksack);

    string[] compartments = new string[]
    {
        rucksack.Substring(0, rucksack.Length / 2),
        rucksack.Substring(rucksack.Length / 2)
    };

    char commonItem = compartments[0].Intersect(compartments[1]).First();

    prioritySum += GetPriority(commonItem);
}

Console.WriteLine($"Part 1: {prioritySum}");

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

