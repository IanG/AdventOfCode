using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Day10
{
    class Program
    {
        private readonly static Dictionary<char, char> bracePairs = new Dictionary<char, char>() 
        {
            { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' }
        };

        private readonly static Dictionary<char, int> corruptBraceScores = new Dictionary<char, int> 
        {
            { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 }
        };

        private readonly static Dictionary<char, int> missingBraceScores = new Dictionary<char, int> 
        {
            { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 }
        };

        static void Main(string[] args)
        {
            List<string> inputs = System.IO.File.ReadLines(args[0]).ToList();
            List<string> corruptLines = GetCorruptedLines(inputs);

            int syntaxErrorScore = 0;
            foreach (string line in corruptLines)
            {
                char illegalBrace = GetIncorrectBrace(line);
                syntaxErrorScore += corruptBraceScores[illegalBrace];
            }            

            Console.WriteLine($"Answer 1: {syntaxErrorScore}");

            List<int> completionScores = new List<int>();
            foreach (string line in inputs.Except(corruptLines))
            {
                int score = 0;
                foreach (char brace in GetMissingBraces(line))
                {
                    score *= 5;
                    score += missingBraceScores[brace];
                }

                completionScores.Add(score);
            }

            int winningScore = completionScores.OrderBy(val => val)
                                .Where((x, i) => i == (completionScores.Count/2))
                                .First();

            Console.WriteLine($"Answer 2: {winningScore}");
        }

        public static List<char> GetMissingBraces(string input)
        {
            List<char> missingBraces = new List<char>();
            Stack<char> braces = new Stack<char>();

            foreach (char c in input)
            {
                if (IsOpeningBrace(c))
                {
                    braces.Push(c);
                }
                else if(IsClosingBrace(c) && IsBracePair(braces.First(), c))
                {
                    braces.Pop();
                }
            }

            foreach (char c in braces)
            {
                missingBraces.Add(bracePairs[c]);
            }

            return missingBraces;
        }
        public static List<string> GetCorruptedLines(List<string> inputs)
        {
            List<string> corruptLines = new List<string>();
            foreach (string input in inputs)
            {
                if (IsCorrupted(input))
                {
                    corruptLines.Add(input);
                }
            }
            return corruptLines;
        }

        private static char GetIncorrectBrace(string input)
        {
            Stack<char> braces = new Stack<char>();
            foreach (char c in input)
            {
                if (IsOpeningBrace(c))
                {
                    braces.Push(c);
                }
                else if(IsClosingBrace(c))
                {
                    if (IsBracePair(braces.First(), c))
                    {
                        braces.Pop();
                    }
                    else
                    {
                        return c;
                    }
                }
            }

            return ' ';
        }
        private static bool IsCorrupted(string input)
        {
            if(IsOpeningBrace(input.First()))
            {
                Stack<char> braces = new Stack<char>();
                
                foreach (char c in input)
                {
                    if (IsOpeningBrace(c))
                    {
                        braces.Push(c);
                    }
                    else if(IsClosingBrace(c))
                    {
                        if (IsBracePair(braces.First(), c))
                        {
                            braces.Pop();
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private static bool IsOpeningBrace(char c)
        {
            return bracePairs.Keys.Contains(c);
        }

        private static bool IsClosingBrace(char c)
        {
            return bracePairs.Values.Contains(c);
        }

        private static bool IsBracePair(char openBrace, char closeBrace)
        {
            return bracePairs[openBrace] == closeBrace;
        }
    }
}
