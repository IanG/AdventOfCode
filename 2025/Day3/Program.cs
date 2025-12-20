Part1("part1puzzleinput.txt");

void Part1(string filename)
{
    int outputVoltage = 0;
    
    foreach (Bank bank in GetBanks(filename))
    {
        int maxJoltage = GetMaxJoltageForBank(bank);
        
        outputVoltage += maxJoltage;
    }
    
    Console.WriteLine($"Part 1: {outputVoltage}");
}

static int GetMaxJoltageForBank(Bank bank)
{
    ReadOnlySpan<int> joltages = bank.Joltages;
 
    // This Bank doesn't have enough batteries to calculate the bank Joltage
    if (joltages.Length < 2) return 0;

    // 1. Find the Peak Joltage ignoring the last battery
    
    int peakJoltage = -1;
    int peakIndex = -1;

    for (int i = 0; i < joltages.Length - 1; i++)
    {
        if (joltages[i] > peakJoltage)
        {
            peakJoltage = joltages[i];
            peakIndex = i;
        }
    }

    // 2. Find the next most powerful battery in the bank beyond the first
    
    int followerJoltage = 0;
    ReadOnlySpan<int> remainingJoltages = joltages[(peakIndex + 1)..];

    foreach (int joltage in remainingJoltages)
    {
        if (joltage > followerJoltage)
        {
            followerJoltage = joltage;
        }
    }

    return (peakJoltage * 10) + followerJoltage;
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
