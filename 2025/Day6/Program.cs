Part1("part1puzzleinput.txt");
static void Part1(string filename)
{
    HomeWork homeWork = GetHomeWorkFromFile(filename);
    long grandTotal = 0;

    foreach (Problem problem in homeWork.Problems)
    {
        switch (problem.Operation)
        {
            case '*':
                grandTotal += Multiply(problem.Numbers);
                break;
            case '+':
                grandTotal += Add(problem.Numbers);
                break;
        }
    }

    Console.WriteLine($"Part 1: {grandTotal}");
}

static long Multiply(long[] numbers)
{
    long total = numbers[0];
    
    for (int i = 1; i < numbers.Length; i++)
    {
        total *= numbers[i];
    }

    return total;
}

static long Add(long[] numbers)
{
    long total = numbers[0];
    
    for (int i = 1; i < numbers.Length; i++)
    {
        total += numbers[i];
    }
    
    return total;
}
static HomeWork GetHomeWorkFromFile(string filename)
{
    string[] lines = File.ReadAllLines(filename);
    if (lines.Length < 2)
        throw new InvalidOperationException("File must have at least one row and one operations row.");

    // Deal with the numbers
    
    int numberRowCount = lines.Length - 1;
    ReadOnlySpan<char> firstLine = lines[0];
    int columnCount = CountColumns(firstLine);
    
    Problem[] problems = new Problem[columnCount];
    long[][] columnarData = new long[columnCount][];
    for (int i = 0; i < columnCount; i++) columnarData[i] = new long[numberRowCount];
    
    for (int y = 0; y < numberRowCount; y++)
    {
        FillRowData(lines[y].AsSpan(), columnarData, y);
    }

    // Deal with the operations
    
    char[] operations = ParseOperations(lines[^1].AsSpan());

    // Assemble the problems 
    
    for (int i = 0; i < columnCount; i++)
    {
        problems[i] = new Problem(columnarData[i], operations[i]);
    }

    return new HomeWork(problems);
}

static void FillRowData(ReadOnlySpan<char> span, long[][] data, int rowIdx)
{
    int colIdx = 0;
    int i = 0;
    while (i < span.Length)
    {
        // Skip whitespace
        while (i < span.Length && char.IsWhiteSpace(span[i])) i++;
        if (i >= span.Length) break;

        int start = i;
    
        // Find end of number
        while (i < span.Length && !char.IsWhiteSpace(span[i])) i++;
            
        // Add the number
        data[colIdx][rowIdx] = long.Parse(span[start..i]);
        colIdx++;
    }
}

static int CountColumns(ReadOnlySpan<char> line)
{
    int count = 0;
    int i = 0;
    while (i < line.Length)
    {
        while (i < line.Length && char.IsWhiteSpace(line[i])) i++;
        if (i < line.Length) count++;
        while (i < line.Length && !char.IsWhiteSpace(line[i])) i++;
    }
    return count;
}

static char[] ParseOperations(ReadOnlySpan<char> span)
{
    int operationCount = CountColumns(span);
    
    char[] operations = new char[operationCount];
    int i = 0;
    
    foreach (char c in span)
    {
        if (!char.IsWhiteSpace(c)) operations[i++] = c;
    }
        
    return operations;
}

readonly record struct HomeWork(Problem[] Problems);
readonly record struct Problem(long[] Numbers, char Operation);