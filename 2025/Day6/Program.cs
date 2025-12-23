Part1("part1puzzleinput.txt"); 
Part2("part2puzzleinput.txt");
static void Part1(string filename)
{
    HomeWork homeWork = GetHomeWorkFromFile(filename);
    long grandTotal = 0;

    foreach (Problem problem in homeWork.Problems)
    {
        long[] numbers = BuildNumbers(problem.NumbersGrid);
        
        switch (problem.Operation)
        {
            case '*':
                grandTotal += Multiply(numbers);
                break;
            case '+':
                grandTotal += Add(numbers);
                break;
        }
    }

    Console.WriteLine($"Part 1: {grandTotal}");
}

static void Part2(string filename)
{
    HomeWork homeWork = GetHomeWorkFromFile(filename);
    long grandTotal = 0;
    
    ReadOnlySpan<Problem> problems = homeWork.Problems.AsSpan();
    
    for (int i = problems.Length - 1; i >= 0; i--)
    {
        long[] numbers = BuildNumbers(problems[i].NumbersGrid, true);
        
        switch (problems[i].Operation)
        {
            case '*':
                grandTotal += Multiply(numbers);
                break;
            case '+':
                grandTotal += Add(numbers);
                break;
        }
    }

    Console.WriteLine($"Part 2: {grandTotal}");
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

    int numberRowCount = lines.Length - 1;
    var numberRows = lines.Take(numberRowCount).ToArray();
    var operationsLine = lines[^1];

    var rowRanges = new List<(int start, int end)>[numberRowCount];
    for (int r = 0; r < numberRowCount; r++)
    {
        rowRanges[r] = GetNumberRanges(numberRows[r].AsSpan());
    }
    
    var columnRanges = MergeColumnRanges(rowRanges);

    int columnCount = columnRanges.Count;
    
    Problem[] problems = new Problem[columnCount];
    char[] operations = ParseOperations(operationsLine.AsSpan());

    for (int c = 0; c < columnCount; c++)
    {
        var (start, end) = columnRanges[c];
        int width = end - start;
        char[][] grid = new char[numberRowCount][];

        for (int r = 0; r < numberRowCount; r++)
        {
            var rowSpan = numberRows[r].AsSpan();
            char[] slice = new char[width];
            int copyLen = Math.Min(width, rowSpan.Length - start);
            if (copyLen > 0)
                rowSpan.Slice(start, copyLen).CopyTo(slice);
            for (int i = copyLen; i < width; i++)
                slice[i] = ' ';
            grid[r] = slice;
        }

        problems[c] = new Problem(grid, operations[c]);
    }

    return new HomeWork(problems);
}

static List<(int start, int end)> GetNumberRanges(ReadOnlySpan<char> line)
{
    var ranges = new List<(int start, int end)>();
    int i = 0;
    while (i < line.Length)
    {
        while (i < line.Length && char.IsWhiteSpace(line[i])) i++;
        if (i >= line.Length) break;
        int start = i;
        while (i < line.Length && !char.IsWhiteSpace(line[i])) i++;
        int end = i;
        ranges.Add((start, end));
    }
    return ranges;
}

static List<(int start, int end)> MergeColumnRanges(List<(int start, int end)>[] rowRanges)
{
    var columns = new List<(int start, int end)>();
    int rowCount = rowRanges.Length;
    
    int[] idx = new int[rowCount];

    while (true)
    {
        int minStart = int.MaxValue;
        bool anyLeft = false;
        
        for (int r = 0; r < rowCount; r++)
        {
            if (idx[r] < rowRanges[r].Count)
            {
                anyLeft = true;
                minStart = Math.Min(minStart, rowRanges[r][idx[r]].start);
            }
        }
        if (!anyLeft) break;
        
        int maxEnd = minStart;
        for (int r = 0; r < rowCount; r++)
        {
            if (idx[r] < rowRanges[r].Count)
            {
                var (s, e) = rowRanges[r][idx[r]];
                if (s <= minStart)
                    maxEnd = Math.Max(maxEnd, e);
            }
        }

        columns.Add((minStart, maxEnd));
        
        for (int r = 0; r < rowCount; r++)
        {
            if (idx[r] < rowRanges[r].Count && rowRanges[r][idx[r]].end <= maxEnd)
                idx[r]++;
        }
    }

    return columns;
}

static char[] ParseOperations(ReadOnlySpan<char> span)
{
    var ops = new List<char>();
    foreach (var c in span)
        if (!char.IsWhiteSpace(c))
            ops.Add(c);
    return ops.ToArray();
}

static long[] BuildNumbers(char[][] numbersGrid, bool cephalopod = false)
{
    int rowCount = numbersGrid.Length;
    int colCount = numbersGrid[0].Length;

    if (!cephalopod)
    {
        // Part 1
        long[] result = new long[rowCount];

        for (int r = 0; r < rowCount; r++)
        {
            long value = 0;

            for (int c = 0; c < colCount; c++)
            {
                char ch = numbersGrid[r][c];
                if (ch >= '0' && ch <= '9')
                {
                    value = value * 10 + (ch - '0');
                }
            }

            result[r] = value;
        }

        return result;
    }
    else
    {
        // Part 2 
        long[] result = new long[colCount];

        int outIndex = 0;

        for (int c = colCount - 1; c >= 0; c--)
        {
            long value = 0;
            long place = 1;

            for (int r = rowCount - 1; r >= 0; r--)
            {
                char ch = numbersGrid[r][c];
                if (ch >= '0' && ch <= '9')
                {
                    value += (ch - '0') * place;
                    place *= 10;
                }
            }

            result[outIndex++] = value;
        }

        return result;
    }
}

readonly record struct HomeWork(Problem[] Problems);
readonly record struct Problem(char[][] NumbersGrid, char Operation);
