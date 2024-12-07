Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");
return;

void Part1(string filename)
{
    List<int[]> input = GetInputData(filename);

    int safeCount = 0;

    foreach (int[] array in input)
    {
        if(AreStrictlySafe(array)) safeCount++;
    }

    Console.WriteLine($"Part 1: {safeCount}");
}

void Part2(string filename)
{
    List<int[]> input = GetInputData(filename);
    
    int safeCount = 0;

    foreach (int[] array in input)
    {
        if (AreSafe(array)) safeCount++;
    }
    
    Console.WriteLine($"Part 2: {safeCount}");
}

bool AreSafe(int[] levels)
{
    if (levels.Length < 2) return false;
    
    if (AreStrictlySafe(levels)) return true;

    for (int i = 0; i < levels.Length; i++)
    {
        int[] adjustedLevels = levels.Where((_, index) => index != i).ToArray();
        if (AreStrictlySafe(adjustedLevels)) return true;
    }

    return false;
}

bool AreStrictlySafe(int[] levels)
{
    const int MINIMUM_DIFFERENCE = 1;
    const int MAXIMUM_DIFFERENCE = 3;
    
    if (levels.Length < 2) return false;

    bool isIncreasing = levels[1] > levels[0];
    
    for (int i = 1; i < levels.Length; i++)
    {
        int difference = levels[i] - levels[i - 1];

        // Check if the difference is within tolerance
        if ((isIncreasing && difference is < MINIMUM_DIFFERENCE or > MAXIMUM_DIFFERENCE) || 
            (!isIncreasing && difference is > MINIMUM_DIFFERENCE * -1 or < MAXIMUM_DIFFERENCE * -1))
        {
            return false;
        }

        // Check if the trend changes
        if ((isIncreasing && difference < 0) || (!isIncreasing && difference > 0)) return false;
    }

    return true;
}
    
List<int[]> GetInputData(string filename)
{
    List<int[]> inputData = [];
    
    foreach (string line in File.ReadLines(filename))
    {
        inputData.Add(Array.ConvertAll(line.Split(' '), int.Parse));
    }

    return inputData;
}