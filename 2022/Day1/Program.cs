IEnumerable<string> lines = System.IO.File.ReadLines(args[0]);

int maxCalories = 0;
int currentCalories = 0;

foreach (string line in lines)
{
    if (String.IsNullOrEmpty(line))
    {
        if (currentCalories > maxCalories)
        {
            maxCalories = currentCalories;
        }
        currentCalories = 0;
    }
    else
    {
        currentCalories += Convert.ToInt32(line);
    }
}

Console.WriteLine($"Max Calories {maxCalories}.");
