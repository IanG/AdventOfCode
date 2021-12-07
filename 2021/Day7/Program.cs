using System;
using System.Collections.Generic;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> inputs = System.IO.File.ReadAllText(args[0])
                .Split(',').Select(value => int.Parse(value)).ToList();

            int lowestCost = GetLowestFuelCostToAlignCrabs(inputs, false);
            Console.WriteLine($"Answer 1: {lowestCost}");

            lowestCost = GetLowestFuelCostToAlignCrabs(inputs, true);
            Console.WriteLine($"Answer 2: {lowestCost}");
        }

        private static int GetLowestFuelCostToAlignCrabs(List<int> crabs, bool slidingFuelCost)
        {
            List<int> costs = new List<int>();

            for (int position = crabs.Min(); position < crabs.Max(); position++)
            {
                int cost = 0;
                for (int crab = 0; crab < crabs.Count; crab++)
                {
                    if (slidingFuelCost)
                    {
                        cost += Enumerable.Range(1, Math.Abs(crabs[crab] - position - 1)).Sum();
                    }
                    else 
                    {
                        cost += Math.Abs(crabs[crab] - position);
                    }
                }
                costs.Add(cost);
            }

            return costs.Min();
        }
    }
}
