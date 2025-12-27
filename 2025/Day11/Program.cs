using System.Collections.Frozen;

Part1("part1puzzleinput.txt");
Part2("part2puzzleinput.txt");

static void Part1(string filename)
{
    ServerRack rack = GetServerRackFromFile(filename);
    
    Device? startNode = rack.GetDevice("you");
    Device? endNode = rack.GetDevice("out");

    if (startNode == null || endNode == null)
    {
        Console.WriteLine("Could not find start or end devices.");
        return;
    }
    
    long totalPaths = CountPaths(startNode, endNode, new());

    Console.WriteLine($"Part 1: {totalPaths}");
}

static void Part2(string filename)
{
    ServerRack rack = GetServerRackFromFile(filename);
    
    Device? svr = rack.GetDevice("svr");
    Device? fft = rack.GetDevice("fft");
    Device? dac = rack.GetDevice("dac");
    Device? @out = rack.GetDevice("out");

    if (svr == null || fft == null || dac == null || @out == null)
    {
        Console.WriteLine("Could not find all devices.");
        return;
    }
    
    // Sequence 1: svr -> fft -> dac -> out
    long s1Leg1 = CountPaths(svr, fft, new());
    long s1Leg2 = CountPaths(fft, dac, new());
    long s1Leg3 = CountPaths(dac, @out, new());
    long scenario1Total = s1Leg1 * s1Leg2 * s1Leg3;

    // Sequence 2: svr -> dac -> fft -> out
    long s2Leg1 = CountPaths(svr, dac, new());
    long s2Leg2 = CountPaths(dac, fft, new());
    long s2Leg3 = CountPaths(fft, @out, new());
    long scenario2Total = s2Leg1 * s2Leg2 * s2Leg3;

    long totalPaths = scenario1Total + scenario2Total;
    
    Console.WriteLine($"Part 2: {totalPaths}");
}

static long CountPaths(Device current, Device target, Dictionary<string, long> memo)
{
    if (current == target) return 1;

    if (memo.TryGetValue(current.Name, out long cachedCount)) return cachedCount;

    long count = 0;
    foreach (Device output in current.Outputs)
    {
        count += CountPaths(output, target, memo);
    }

    memo[current.Name] = count;
    return count;
}

static ServerRack GetServerRackFromFile(string filename)
{
    const char serverSeparator = ':';
    Dictionary<string, Device> registry = new Dictionary<string, Device>();

    foreach (string line in File.ReadLines(filename))
    {
        ReadOnlySpan<char> lineSpan = line.AsSpan();
        int separatorIndex = lineSpan.IndexOf(serverSeparator);
            
        string name = lineSpan[..separatorIndex].ToString();
        ReadOnlySpan<char> outputsSpan = lineSpan[(separatorIndex + 2)..];

        if (!registry.TryGetValue(name, out Device? device))
        {
            device = new Device(name, []);
            registry[name] = device;
        }

        foreach (Range outputRange in outputsSpan.Split(' '))
        {
            if (outputRange.Start.Value == outputRange.End.Value) continue;

            string outputDeviceName = outputsSpan[outputRange].ToString();
                
            if (!registry.TryGetValue(outputDeviceName, out Device? outputDevice))
            {
                outputDevice = new Device(outputDeviceName, []);
                registry[outputDeviceName] = outputDevice;
            }
                
            device.Outputs.Add(outputDevice);
        }
    }
    
    return new ServerRack(registry.ToFrozenDictionary());
}

readonly record struct ServerRack(FrozenDictionary<string, Device> Devices)
{
    public Device? GetDevice(string name) => Devices.GetValueOrDefault(name);
}

record Device(string Name, List<Device> Outputs)
{
    public override string ToString() => $"{Name} -> {Outputs.Count} links";
}