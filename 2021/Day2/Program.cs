using System;
using System.Collections.Generic;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<KeyValuePair<string, int>> actions = System.IO.File.ReadLines(args[0])
                .Select(line => new { values = line.Split(' ')})
                .Select(line => new KeyValuePair<string, int>(line.values[0], int.Parse(line.values[1])))
                .ToList();
            
            int hPos = 0, depth = 0;
            foreach (KeyValuePair<string, int> action in actions)
            {
                switch (action.Key)
                {
                    case "forward": 
                        hPos += action.Value;
                        break;
                    case "down":
                        depth += action.Value;
                        break;
                    case "up":
                        depth -= action.Value;
                        break;
                }
            }

            Console.WriteLine($"Part 1: {hPos * depth}");

            int aim = 0;
            hPos = depth = 0;
            foreach (KeyValuePair<string, int> action in actions)
            {
                switch (action.Key)
                {
                    case "forward": 
                        hPos += action.Value;
                        depth += aim * action.Value;
                        break;
                    case "down":
                        aim += action.Value;
                        break;
                    case "up":
                        aim -= action.Value;  
                        break;
                }
            }

            Console.WriteLine($"Part 2: {hPos * depth}");
        }
    }
}
