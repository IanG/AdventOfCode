using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            List<DotCoordinate> dotCoordinates = GetDotCoordinates(args[0]);
            List<FoldInstruction> foldInstructions = GetFoldInstrucutions(args[0]);

            Paper paper = new Paper(dotCoordinates);

            paper.Fold(foldInstructions[0]);

            Console.WriteLine($"Answer 1: {paper.VisibleDots}");

            paper = new Paper(dotCoordinates);
            foreach (FoldInstruction foldInstruction in foldInstructions)
            {
                paper.Fold(foldInstruction);
            }
            Console.WriteLine("Answer 2:");
            paper.Print();

        }
        
        static List<FoldInstruction> GetFoldInstrucutions(string input)
        {
            const string FOLDINSTRUCTION_REGEX = @"^fold along (x|y)=(\d*)$";
            Regex regex = new Regex(FOLDINSTRUCTION_REGEX, RegexOptions.IgnoreCase);

            List<FoldInstruction> foldInstructions = new List<FoldInstruction>();

            List<string> foldLines = System.IO.File.ReadLines(input)
                .Where(line => regex.IsMatch(line))
                .ToList();

            foreach (string foldLine in foldLines)
            {
                MatchCollection matches = Regex.Matches(foldLine, FOLDINSTRUCTION_REGEX);
                GroupCollection groups = matches[0].Groups;

                string axis = groups[1].Value;
                int line = Int32.Parse(groups[2].Value);

                if (axis.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    foldInstructions.Add(new VerticalFoldInstruction(line));
                }
                else
                {
                    foldInstructions.Add(new HorizontalFoldInstruction(line));
                }
            }

            return foldInstructions;
        }
        static List<DotCoordinate> GetDotCoordinates(string input)
        {
            const string COORDINATE_REGEX = @"^(\d*),(\d*)$";
            Regex regex = new Regex(COORDINATE_REGEX, RegexOptions.IgnoreCase);

            List<DotCoordinate> dotCoordinates = new List<DotCoordinate>();

            List<string> coordinateLines = System.IO.File.ReadLines(input)
                .Where(coordinate => regex.IsMatch(coordinate))
                .ToList();
                
            foreach (string line in coordinateLines)
            {
                MatchCollection matches = Regex.Matches(line, COORDINATE_REGEX);
                GroupCollection groups = matches[0].Groups;

                int x = Int32.Parse(groups[1].Value);
                int y = Int32.Parse(groups[2].Value);

                dotCoordinates.Add(new DotCoordinate { X = x, Y = y });
            }

            return dotCoordinates;
        }

        public class Paper
        {
            bool[,] paper;

            public Paper(List<DotCoordinate> dots)
            {
                paper = new bool[dots.Select(dot => dot.Y).Max() + 1, dots.Select(dot => dot.X).Max() + 1];

                foreach (DotCoordinate dot in dots)
                {
                    paper[dot.Y, dot.X] = true;
                }
            }

            public int VisibleDots 
            {
                get
                {
                    int visibleDots = 0;
                    for (int y = 0; y <= paper.GetUpperBound(0); y++)
                    {
                        for (int x = 0; x <= paper.GetUpperBound(1); x++)
                        {
                            if (paper[y, x]) visibleDots++;
                        }
                    }
                    return visibleDots;
                }
            }
            public void Fold(FoldInstruction instruction)
            {
                if (instruction is VerticalFoldInstruction)
                {
                    FoldVertical(instruction.Point);
                }
                else if (instruction is HorizontalFoldInstruction)
                {
                    FoldHorizontal(instruction.Point);
                }
            }

            private void FoldVertical(int yPos)
            {
                Console.WriteLine($"BEFORE Grid is {paper.GetUpperBound(0) + 1} x {paper.GetUpperBound(1) + 1} Folding Vertical at Y {yPos}");
                
                int upperY = yPos - 1;
                for (int y = yPos + 1; y <= paper.GetUpperBound(0); y++)
                {
                    for (int x = 0; x <= paper.GetUpperBound(1); x++)
                    {
                        if (paper[y, x]) paper[upperY, x] = true;
                    }
                    upperY--;
                }

                ResizePaper(paper.GetUpperBound(1) + 1, paper.GetUpperBound(0) - yPos);
                Console.WriteLine($"AFTER  Grid is {paper.GetUpperBound(0) + 1} x {paper.GetUpperBound(1) + 1}");
            }

            private void FoldHorizontal(int xPos)
            {
                if(xPos * 2 != paper.GetUpperBound(1))
                    Console.WriteLine("Hello");

                Console.WriteLine($"BEFORE Grid is {paper.GetUpperBound(0) + 1} x {paper.GetUpperBound(1) + 1} Folding Horizontal at X {xPos}");
                for (int y = 0; y <= paper.GetUpperBound(0); y++)
                {
                    int upperX = xPos - 1;
                    for (int x = xPos + 1; x <= paper.GetUpperBound(1); x++)
                    {
                        if (paper[y, x]) paper[y, upperX] = true;
                        upperX--;
                    }
                }

                ResizePaper(paper.GetUpperBound(1) - xPos , paper.GetUpperBound(0) + 1);
                Console.WriteLine($"AFTER  Grid is {paper.GetUpperBound(0) + 1} x {paper.GetUpperBound(1) + 1}");
            }

            private void ResizePaper(int newX, int newY)
            {
                bool[,] newArray = new bool[newY, newX];
                
                int minY = Math.Min(paper.GetLength(0), newArray.GetLength(0));
                int minX = Math.Min(paper.GetLength(1), newArray.GetLength(1));

                for (int y = 0; y < minY; y++)
                {
                    Array.Copy(paper, y * paper.GetLength(1), newArray, y * newArray.GetLength(1), minX);
                }

                paper = newArray;
            }

            public void Print()
            {
                Console.WriteLine($"Paper is {paper.GetUpperBound(0) + 1} x {paper.GetUpperBound(1) + 1}");
                Console.WriteLine($"{new String('=', paper.GetUpperBound(1) + 1)}");
                for (int y = 0; y <= paper.GetUpperBound(0); y++)
                {
                    for (int x = 0; x <= paper.GetUpperBound(1); x++)
                    {
                        if (paper[y, x])
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"{new String('=', paper.GetUpperBound(1) + 1)}");    
            }
        }
             
        public abstract class FoldInstruction 
        {
            public int Point { get; }

            public FoldInstruction(int point)
            {
                Point = point;
            }
        }

        public class VerticalFoldInstruction : FoldInstruction
        {
            public VerticalFoldInstruction(int point) : base(point) {}
        }

        public class HorizontalFoldInstruction : FoldInstruction
        {
            public HorizontalFoldInstruction(int point) : base(point) {}
        }

        public class DotCoordinate
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
                if (!(obj is DotCoordinate)) return false;

                DotCoordinate other = (DotCoordinate)obj;

                return X == other.X && Y == other.Y;
            }

            public override int GetHashCode() 
            {
                return X.GetHashCode() + Y.GetHashCode();
            }
        }
    }

    
}
