using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<string> inputs = System.IO.File.ReadLines(args[0]).ToList();

            List<HydrothermalVent> vents = null;
            int overlappingPoints = 0;

            vents = GetHydrothermalVents(inputs, false);
            overlappingPoints = GetOverlappingPoints(vents);
            
            Console.WriteLine($"Part 1: {overlappingPoints}");

            vents = GetHydrothermalVents(inputs, true);
            overlappingPoints = GetOverlappingPoints(vents);

            Console.WriteLine($"Part 2: {overlappingPoints}");
        }

        private static List<HydrothermalVent> GetHydrothermalVents(IList<string> readings, bool includeDiagonals)
        {
            string READING_REGX = @"^(\d*),(\d*) -> (\d*),(\d*)$";

            List<HydrothermalVent> vents = new List<HydrothermalVent>();

            foreach (string reading in readings)
            {
                MatchCollection matches = Regex.Matches(reading, READING_REGX);

                if (matches.Count > 0 && matches[0].Groups.Count > 0)
                {
                    GroupCollection groups = matches[0].Groups;

                    Coordinate start = new Coordinate { X = Int32.Parse(groups[1].Value), Y = Int32.Parse(groups[2].Value) };
                    Coordinate end = new Coordinate { X = Int32.Parse(groups[3].Value), Y = Int32.Parse(groups[4].Value) };

                    List<Coordinate> coordinates = GetVentCoordinates(start, end, includeDiagonals);

                    if (coordinates.Count > 0) vents.Add(new HydrothermalVent { Coordinates = coordinates });
                }
            }

            return vents;
        }

        public static List<Coordinate> GetVentCoordinates(Coordinate start, Coordinate end, bool includeDiagonals)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            int x = start.X;
            int y = start.Y;
            
            if (start.X == end.X)
            {
                coordinates.Add(new Coordinate { X = x, Y = y});    
                while (y != end.Y)
                {
                    y = start.Y < end.Y ? ++y : --y;
                    coordinates.Add(new Coordinate { X = x, Y = y});
                }
            }
            else if (start.Y == end.Y)
            {
                coordinates.Add(new Coordinate { X = x, Y = y});
                while (x != end.X)
                {
                    x = start.X < end.X ? ++x : --x;
                    coordinates.Add(new Coordinate { X = x, Y = y});
                }
            }
            else
            {
                if (includeDiagonals)
                {
                    coordinates.Add(new Coordinate { X = x, Y = y});    
                    while (x != end.X && y != end.Y)
                    {
                        x = start.X < end.X ? ++x : --x; 
                        y = start.Y < end.Y ? ++y : --y;
                        coordinates.Add(new Coordinate { X = x, Y = y});
                    } 
                }
            }

            return coordinates;
        }
        
        private static int GetOverlappingPoints(List<HydrothermalVent> vents)
        {
            Dictionary<Coordinate, int> ventPoints = new Dictionary<Coordinate, int>();

            foreach (HydrothermalVent vent in vents)
            {
                foreach (Coordinate ventCoordinate in vent.Coordinates)
                {
                    if (ventPoints.ContainsKey(ventCoordinate))
                    {
                        ventPoints[ventCoordinate]++;
                    }
                    else
                    {
                        ventPoints.Add(ventCoordinate, 1);
                    }
                }
            }
            return ventPoints.Where(point => point.Value > 1).Count();
        }
        
        public class HydrothermalVent
        {
            public List<Coordinate> Coordinates { get; set; }
        }

        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }

            public override string ToString() 
            { 
                return $"X={X}, Y={Y}"; 
            }

            public override bool Equals(Object obj) 
            {
                if (obj == null) return false;
                if (!(obj is Coordinate)) return false;

                Coordinate other = (Coordinate)obj;

                return X == other.X && Y == other.Y;
            }

            public override int GetHashCode() 
            {
                return X.GetHashCode() + Y.GetHashCode();
            }
        }
    }
}
