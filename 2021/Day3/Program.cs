using System;
using System.Collections.Generic;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<string> diagnostics = System.IO.File.ReadLines(args[0]).ToList();
            
            int gammaRate = GetGammaRate(diagnostics);
            int epsilonRate = GetEpsilonRate(diagnostics);

            Console.WriteLine($"Part 1: {gammaRate * epsilonRate}");

            int oxygenGeneratorRating = GetOxygenGeneratorRating(diagnostics);
            int cO2ScrubberRating = GetCO2ScrubberRating(diagnostics);
            
            Console.WriteLine($"Part 2: {oxygenGeneratorRating * cO2ScrubberRating}");
        }

        private static int GetGammaRate(IList<string> diagnostics)
        {
            int diagnosticCount = diagnostics.Count();
            int[] oneCounts = GetOneCounts(diagnostics);

            string gamma = "";
            for (int bit = 0; bit < oneCounts.Length; bit++)
            {
                gamma += oneCounts[bit] > (diagnosticCount / 2) ? '1' : '0';
            }

            return Convert.ToInt32(gamma, 2);
        }

        private static int GetEpsilonRate(IList<string> diagnostics)
        {
            int diagnosticCount = diagnostics.Count();
            int[] oneCounts = GetOneCounts(diagnostics);

            string epsilon = "";
            for (int bit = 0; bit < oneCounts.Length; bit++)
            {
                epsilon += oneCounts[bit] > (diagnosticCount / 2) ? '0' : '1';
            }

            return Convert.ToInt32(epsilon, 2);
        }

        private static int GetOxygenGeneratorRating(IList<string> diagnostics)
        {
            int bitPos = 0;
            while (diagnostics.Count > 1)
            {
                int oneCount = GetOneCount(diagnostics, bitPos);
                int zeroCount = diagnostics.Count - oneCount;

                char filter = (oneCount > zeroCount) || (oneCount == zeroCount) ? '1' : '0';
                diagnostics = diagnostics.Where(diagnostic => diagnostic[bitPos] == filter).ToList();
                
                bitPos++;
            }

            return Convert.ToInt32(diagnostics.Last(), 2);
        }

        private static int GetCO2ScrubberRating(IList<string> diagnostics)
        {
            int bitPos = 0;
            while (diagnostics.Count > 1)
            {
                int oneCount = GetOneCount(diagnostics, bitPos);
                int zeroCount = diagnostics.Count - oneCount;
                
                char filter = (zeroCount < oneCount) || (oneCount == zeroCount) ? '0' : '1';
                diagnostics = diagnostics.Where(diagnostic => diagnostic[bitPos] == filter).ToList();
                
                bitPos++;
            }

            return Convert.ToInt32(diagnostics.Last(), 2);
        }

        private static int GetOneCount(IList<string> diagnostics, int bitPosition)
        {
            return (int)diagnostics.Select(diagnostic => Char.GetNumericValue(diagnostic[bitPosition])).Sum();
        }

        private static int[] GetOneCounts(IList<string> diagnostics)
        {
            int[] bitCounts = new int[diagnostics.First().Length];
            foreach (string diagnostic in diagnostics)
            {
                int[] bits = diagnostic.Select(bit => Int32.Parse(bit.ToString())).ToArray();
                for (int bit = 0; bit < bits.Length; bit++) bitCounts[bit] += bits[bit];
            }

            return bitCounts;
        }
    }
}
