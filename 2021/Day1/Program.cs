using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> depthMeasurements = System.IO.File.ReadLines(args[0]).Select(line => int.Parse(line));

            int previousDepth = 0, depthIncreases = 0;
            foreach (int depth in depthMeasurements)
            {
                if (previousDepth > 0 && depth > previousDepth) depthIncreases++;
                previousDepth = depth;
            }

            Console.WriteLine($"Part 1: {depthIncreases}");

            previousDepth = depthIncreases = 0;
            for (int pos = 0; pos < depthMeasurements.Count(); pos++)
            {
                int window = depthMeasurements.Skip(pos).Take(3).Sum();
                if (previousDepth > 0 && window > previousDepth) depthIncreases++;
                previousDepth = window;
            }

            Console.WriteLine($"Part 2: {depthIncreases}");
        }
    }
}
