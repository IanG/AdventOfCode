using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day11
{
    class Program
    {
        private const int FLASH_LEVEL = 10;

        static void Main(string[] args)
        {
            int flashCount = GetFlashesFromSteps(getOctopi(args[0]), 100);
            Console.WriteLine($"Answer 1: {flashCount}");

            int flashesSynchroniseAt = GetStepWhenFlashesSynchronise(getOctopi(args[0]));
            Console.WriteLine($"Answer 2: {flashesSynchroniseAt}");
        }

        public static int GetStepWhenFlashesSynchronise(int[,] octopi)
        {
            Console.WriteLine("Start");
            ShowOctopi(octopi);

            int step = 0;
            while(true)
            {
                RaiseOctopiEnergyLevelsBy(octopi, 1);
                FlashOctopi(octopi);

                Console.WriteLine($"After step {step + 1}:");
                ShowOctopi(octopi);
                step++;

                if (AllOctopiJustFlashed(octopi)) return step;                
            }
        }

        public static int GetFlashesFromSteps(int[,] octopi, int steps)
        {
            int totalFlashes = 0;

            Console.WriteLine("Start");
            ShowOctopi(octopi);

            for (int step = 0; step < steps; step++)
            {
                RaiseOctopiEnergyLevelsBy(octopi, 1);
                totalFlashes += FlashOctopi(octopi);

                Console.WriteLine($"After step {step + 1}:");
                ShowOctopi(octopi);
            }

            return totalFlashes;
        }

        public static int FlashOctopi(int[,] octopi)
        {
            List<Coordinate> flashedOctopi = new List<Coordinate>();

            List<Coordinate> octopiToFlash = GetOctopiToFlash(octopi);
            while (octopiToFlash.Count > 0)
            {
                foreach (Coordinate coordinate in octopiToFlash)
                {
                    FlashOctopusAt(octopi, coordinate);
                }

                flashedOctopi.AddRange(octopiToFlash);
                octopiToFlash = GetOctopiToFlash(octopi).Except(flashedOctopi).ToList();
            }

            ResetOctopiAt(octopi, flashedOctopi);
            return flashedOctopi.Count();
        }

        public static bool AllOctopiJustFlashed(int[,] octopi)
        {
            for (int x = 0; x < octopi.GetLength(0); x++)
            {
                for (int y = 0; y < octopi.GetLength(1); y++)
                {
                    if (octopi[x,y] != 0) return false;
                }
            }
            return true;
        }

        public static void ResetOctopiAt(int[,] octopi, List<Coordinate> coordinates)
        {
            foreach (Coordinate coordinate in coordinates)
            {
                octopi[coordinate.X, coordinate.Y] = 0;
            }
        }

        public static void FlashOctopusAt(int[,] octopi, Coordinate coordinate)
        {
            foreach (Coordinate neighbour in GetNeighboursFromPosition(octopi, coordinate))
            {
                if (octopi[neighbour.X, neighbour.Y] < FLASH_LEVEL)
                {
                    octopi[neighbour.X, neighbour.Y]++;
                }
            }
        }

        public static List<Coordinate> GetOctopiToFlash(int[,] octopi)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            for (int x = 0; x < octopi.GetLength(0); x++)
            {
                for (int y = 0; y < octopi.GetLength(1); y++)
                {
                    if (octopi[x,y] == FLASH_LEVEL) 
                    {
                        coordinates.Add(new Coordinate(x, y));
                    }
                }
            }
            return coordinates;
        }

        public static void RaiseOctopiEnergyLevelsBy(int[,] octopi, int level)
        {
            for (int x = 0; x < octopi.GetLength(0); x++)
            {
                for (int y = 0; y < octopi.GetLength(1); y++)
                {
                    if (octopi[x,y] < FLASH_LEVEL) octopi[x,y] += level;
                }
            }

            ShowOctopi(octopi);
        }

        public static List<Coordinate> GetNeighboursFromPosition(int[,] map, Coordinate position)
        {
            List<Coordinate> neighbours = new List<Coordinate>();

            neighbours.Add(new Coordinate(position.X - 1, position.Y - 1)); // Top Left
            neighbours.Add(new Coordinate(position.X, position.Y - 1));     // Above
            neighbours.Add(new Coordinate(position.X + 1, position.Y - 1)); // Top Right
            neighbours.Add(new Coordinate(position.X - 1, position.Y));     // Left
            neighbours.Add(new Coordinate(position.X + 1, position.Y));     // Right
            neighbours.Add(new Coordinate(position.X - 1, position.Y + 1)); // Bottom Left
            neighbours.Add(new Coordinate(position.X, position.Y + 1));     // Below
            neighbours.Add(new Coordinate(position.X + 1, position.Y + 1)); // Bottom Right
            
            return neighbours.Where(cord => ((cord.X > -1 && cord.X < map.GetLength(0)) && 
                (cord.Y > -1 && cord.Y < map.GetLength(1)))).ToList();
        }

        public static int[,] getOctopi(string inputFile)
        {
            List<string> inputs = System.IO.File.ReadLines(inputFile).ToList();

            int[,] octopi = new int[inputs.Count, inputs.Count];

            int x = 0;
            foreach (string line in inputs)
            {
                int y = 0;
                foreach (char digit in line)
                {
                    octopi[x,y] = (int)Char.GetNumericValue(digit);
                    y++;
                }
                x++;
            }
            return octopi;
        }

        private static void ShowOctopi(int[,] octopi)
        {
            for (int x = 0; x < octopi.GetLength(0); x++)
            {
                for (int y = 0; y < octopi.GetLength(1); y++)
                {
                    if (octopi[x,y] == 0)
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    else if (octopi[x,y] == FLASH_LEVEL)
                        Console.ForegroundColor = ConsoleColor.Green;    
                    else
                        Console.ForegroundColor = ConsoleColor.White;
                    
                    Console.Write($"{octopi[x,y]:D2} ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine(new String('-', (octopi.GetLength(0) * 2) + octopi.GetLength(0) - 1));
        }

        public struct Coordinate
        {
            public Coordinate(int x, int y) { X = x; Y = y; }
            public int X { get; set; }
            public int Y { get; set; }

            public override string ToString() { return $"X={X}, Y={Y}"; }
        } 
    }
}
