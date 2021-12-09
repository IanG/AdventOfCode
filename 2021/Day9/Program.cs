using System;
using System.Collections.Generic;
using System.Linq;

namespace Day9
{
    class Program
    {       
        static void Main(string[] args)
        {
            int[][] heightMap = System.IO.File.ReadLines(args[0])
                .Select(line => line.ToCharArray().Select(c => (int)Char.GetNumericValue(c)).ToArray())
                .ToArray();

            List<Coordinate> lowPoints = GetLowPoints(heightMap);
            int riskLevel = GetRiskLevelForLowPoints(heightMap, lowPoints).Sum();

            Console.WriteLine($"Part 1: {riskLevel}");
        }
        
        public static List<int> GetRiskLevelForLowPoints(int[][] heightMap, List<Coordinate> lowPoints)
        {
            List<int> riskLevels = new List<int>();

            foreach (Coordinate lowPoint in lowPoints)
            {
                riskLevels.Add(heightMap[lowPoint.Y][lowPoint.X] + 1);
            }
            return riskLevels;
        }

        public static List<Coordinate> GetLowPoints(int[][] heightMap)
        {
            List<Coordinate> lowPoints = new List<Coordinate>();

            for (int y = 0; y < heightMap.Length; y++) 
            {
                for (int x = 0; x < heightMap[0].Length; x++)    
                {
                    Coordinate currentPosition = new Coordinate(x, y);
                    int currentHeight = heightMap[y][x];

                    List<Coordinate> neighbours = GetNeighboursFromPosition(heightMap, currentPosition);
                    List<int> heights = GetHeightsForNeighbours(heightMap, neighbours);

                    if (heights.All(height => height > currentHeight))
                    {
                        lowPoints.Add(currentPosition);
                    }
                }
            }

            return lowPoints;
        }
        
        public static List<int> GetHeightsForNeighbours(int[][] heightMap, List<Coordinate> neighbours)
        {
            List<int> neighBourHeights = new List<int>();

            foreach (Coordinate neighbour in neighbours)
            {
                neighBourHeights.Add(heightMap[neighbour.Y][neighbour.X]);
            }

            return neighBourHeights;
        }

        public static List<Coordinate> GetNeighboursFromPosition(int[][] map, Coordinate position)
        {
            List<Coordinate> neighbours = new List<Coordinate>();

            // neighbours.Add(new Coordinate(position.X - 1, position.Y - 1)); // Top Left
            neighbours.Add(new Coordinate(position.X, position.Y - 1));        // Above
            // neighbours.Add(new Coordinate(position.X + 1, position.Y - 1)); // TopRight
            neighbours.Add(new Coordinate(position.X - 1, position.Y));        // Left
            neighbours.Add(new Coordinate(position.X + 1, position.Y));        // Right
            // neighbours.Add(new Coordinate(position.X - 1, position.Y + 1)); // Bottom Left
            neighbours.Add(new Coordinate(position.X, position.Y + 1));        // Below
            // neighbours.Add(new Coordinate(position.X + 1, position.Y + 1)); // Bottom Right

            return neighbours.Where(cord => ((cord.X > -1 && cord.X < map[0].Length) && 
                (cord.Y > -1 && cord.Y < map.Length))).ToList();
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
