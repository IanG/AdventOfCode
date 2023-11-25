IEnumerable<string> lines = System.IO.File.ReadLines(args[0]);

int currentCalories = 0;

List<int> elfs = new List<int>();
foreach (string line in lines)
{
    if (String.IsNullOrEmpty(line))
    {
        elfs.Add(currentCalories);
        currentCalories = 0;
    }
    else
    {
        currentCalories += Convert.ToInt32(line);
    }
}

Console.WriteLine($"Part 1: {elfs.Max()}");
Console.WriteLine($"Part 2: {elfs.OrderDescending().Take(3).Sum()}");