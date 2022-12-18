using System.Text.RegularExpressions;

IEnumerable<string> input = System.IO.File.ReadLines(args[0]);

Stack<char>[] stacks = GetStartingStacks(input);
List<(int howmany, int source, int destination)> instructions = GetInstructions(input);

foreach ((int howmany, int source, int destination) instruction in instructions)
{
    PushTo(stacks[instruction.destination], PopFrom(stacks[instruction.source], instruction.howmany));
}
Console.WriteLine($"Part 1: {new String(GetStackTops(stacks).ToArray())}");

stacks = GetStartingStacks(input);

foreach ((int howmany, int source, int destination) instruction in instructions)
{
    PushTo(stacks[instruction.destination], PopFrom(stacks[instruction.source], instruction.howmany, true));
}
Console.WriteLine($"Part 2: {new String(GetStackTops(stacks).ToArray())}");

IEnumerable<char> GetStackTops(Stack<char>[] stacks)
{
    for (int stack = 0; stack < stacks.Length; stack++) yield return stacks[stack].Peek();
}

IEnumerable<char> PopFrom(Stack<char> stack, int howmany, bool reversed = false)
{
    List<char> poppedChars = new List<Char>();
    while (howmany > 0)
    {
        poppedChars.Add(stack.Pop());
        howmany--;
    }

    if (reversed) poppedChars.Reverse();
    return poppedChars;
}

void PushTo(Stack<char> stack, IEnumerable<char> chars)
{
    foreach (char c in chars) stack.Push(c);
}

List<(int howmany, int source, int destination)> GetInstructions(IEnumerable<string> input)
{
    const string REGEX_PATTERN = @"^move (?<howmany>\d*) from (?<source>\d*) to (?<destination>\d*)$";
    Regex regex = new Regex(REGEX_PATTERN);
    bool capturing = false;

    List<(int howmany, int source, int destination)> instructions = new ();
    foreach (string line in input)
    {
        if (capturing) 
        {
            Match match = regex.Match(line);

            if (match.Success)
            {
                instructions.Add((
                    howmany: Convert.ToInt16(match.Groups["howmany"].Value),
                    source: Convert.ToInt16(match.Groups["source"].Value) - 1,
                    destination: Convert.ToInt16(match.Groups["destination"].Value) - 1
                ));
            }
        }

        if (!capturing && String.IsNullOrWhiteSpace(line)) capturing = true;
    }

    return instructions;
}

Stack<char>[] GetStartingStacks(IEnumerable<string> input)
{
    string[] start = RotateStart(GetStart(input));
    Stack<char>[] stacks = new Stack<char>[start.Length];

    for (int line = 0; line < start.Length; line++)
    {
        stacks[line] = new Stack<char>();
        foreach (char c in start[line]) stacks[line].Push(c);
    }

    return stacks;
}

string[] GetStart(IEnumerable<string> input)
{
    List<string> start = new List<string>();
    foreach (string line in input)
    {
        if (!String.IsNullOrWhiteSpace(line))
            start.Add(line);
        else
            break;
    }

    start.Reverse();
    return start.ToArray();
}

string[] RotateStart(string[] start)
{
    string[] rotatedLines = new string[start[0].Length];
    foreach (string line in start)
    {
        for (int c = 0; c < line.Length; c++)
        {
            rotatedLines[c] += line[c];
        }
    }

    return rotatedLines.Where(line => !line.StartsWith(" "))
        .Select(item => { return item.Trim().Substring(1); })
        .ToArray();
}
