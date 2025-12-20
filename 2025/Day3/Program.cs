Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");

void Part1(string filename)
{
    const int batteryCount = 2;
    long outputVoltage = 0;
    
    foreach (Bank bank in GetBanks(filename))
    {
        long maxJoltage = GetMaxJoltageForBank(bank, batteryCount);
        
        outputVoltage += maxJoltage;
    }
    
    Console.WriteLine($"Part 1: {outputVoltage}");
}

void Part2(string filename)
{
    const int batteryCount = 12;
    long outputVoltage = 0;
    
    foreach (Bank bank in GetBanks(filename))
    {
        long maxJoltage = GetMaxJoltageForBank(bank, batteryCount);
        
        outputVoltage += maxJoltage;
    }
    
    Console.WriteLine($"Part 2: {outputVoltage}");
}

static long GetMaxJoltageForBank(Bank bank, int batteryCount)
{
    ReadOnlySpan<int> joltages = bank.Joltages;
    
    if (joltages.Length < batteryCount) return 0;
        
    long totalVoltage = 0;
    int currentStartIndex = 0;

    for (int b = 0; b < batteryCount; b++)
    {
        int searchLimit = joltages.Length - (batteryCount - 1 - b);
            
        int peakValue = -1;
        int peakIndex = -1;

        for (int i = currentStartIndex; i < searchLimit; i++)
        {
            if (joltages[i] > peakValue)
            {
                peakValue = joltages[i];
                peakIndex = i;
            }
        }
        
        totalVoltage = (totalVoltage * 10) + peakValue;
        currentStartIndex = peakIndex + 1;
    }

    return totalVoltage;
}

static IEnumerable<Bank> GetBanks(string filename)
{
    foreach (string line in File.ReadLines(filename))
    {
        yield return GetBank(line);    
    }
}

static Bank GetBank(string bankData)
{
    int length = bankData.Length;
    int[] joltages = new int[length];
        
    for (int i = 0; i < length; i++)
    {
        joltages[i] = bankData[i] - '0';
    }

    return new Bank() { Joltages = joltages };
}

sealed class Bank
{
    public int[] Joltages { get; init; } = [];
}
