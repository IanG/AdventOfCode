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

            int lowestCost = GetLowestFuelCostToAlignCrabs(inputs);

            Console.WriteLine($"Answer 1: {lowestCost}");
        }

        private static int GetLowestFuelCostToAlignCrabs(List<int> crabs)
        {
            List<int> costs = new List<int>();

            foreach (int position in crabs.Distinct())
            {
                int cost = 0;
                for (int crab = 0; crab < crabs.Count; crab++)
                {
                    cost+= Math.Abs(crabs[crab] - position);
                }
                costs.Add(cost);
            }

            return costs.Min();
        }
    }
}
