IEnumerable<string> assignmentPairs = System.IO.File.ReadLines(args[0]);

int fullyContainedAssigmentPairs = 0;
int overlappingAssignmentPairs = 0;

foreach (string assignmentPair in assignmentPairs)
{
    string[] elfAssignments = assignmentPair.Split(',');

    List<int> firstElfSections = GetSections(elfAssignments[0]);
    List<int> secondElfSections = GetSections(elfAssignments[1]);

    if (SectionsOverlap(firstElfSections, secondElfSections))
    {
        overlappingAssignmentPairs++;
        if (SectionContainsOther(firstElfSections, secondElfSections))
        {
            fullyContainedAssigmentPairs++;
        }
    }
}

Console.WriteLine($"Part 1 : {fullyContainedAssigmentPairs}");
Console.WriteLine($"Part 2 : {overlappingAssignmentPairs}");

bool SectionsOverlap(List<int> first, List<int> second)
{
    return first.Intersect(second).Count() > 0;
}

bool SectionContainsOther(List<int> first, List<int> second)
{
    return first.Intersect(second).Count() == first.Count() ||
        second.Intersect(first).Count() == second.Count();
}

List<int> GetSections(string sectionRange)
{
    string[] boundaries = sectionRange.Split('-');

    int firstSection = Convert.ToInt16(boundaries[0]);
    int lastSection = Convert.ToInt16(boundaries[1]);

    return Enumerable.Range(firstSection, (lastSection - firstSection) + 1).ToList();
}
