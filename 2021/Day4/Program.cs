using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day4
{
    class Program
    {
        
        static void Main(string[] args)
        {
            IList<string> inputs = System.IO.File.ReadLines(args[0]).ToList();

            Queue<int> numbers = GetPlayQueue(inputs);
            List<Board> boards = GetBoards(inputs);

            List<int> playedNumbers = new List<int>();
            Board winningBoard = null;
            bool bingo = false;

            while (numbers.Count > 0 && !bingo)
            {
                playedNumbers.Add(numbers.Dequeue());
                PlayNumber(boards, playedNumbers.Last());

                winningBoard = GetWinningBoard(boards);

                if (winningBoard != null) bingo = true;
            }

            int remainingSum = winningBoard.GetRemainingSum();
            int lastNumber = playedNumbers.Last();

            Console.WriteLine($"Part 1: {remainingSum * lastNumber}");

            numbers = GetPlayQueue(inputs);
            boards = GetBoards(inputs);
            playedNumbers = new List<int>();

            List<Board> winningBoards = new List<Board>();
            
            while (numbers.Count > 0)
            {
                playedNumbers.Add(numbers.Dequeue());
                PlayNumber(boards, playedNumbers.Last());

                List<Board> winners = GetWinningBoards(boards);

                if (winners != null) 
                {
                    foreach (Board board in winners)
                    {
                        winningBoards.Add(board);
                        boards.Remove(board);
                    }
                    if (boards.Count == 0) break;
                }
            }

            Board lastWinner = winningBoards.Last();
            remainingSum = lastWinner.GetRemainingSum();
            lastNumber = playedNumbers.Last();
            
            Console.WriteLine($"Part 2: {remainingSum * lastNumber}");
        }

        private static void PlayNumber(List<Board> boards, int number)
        {
            boards.ForEach(board => board.Play(number));
        }

        private static List<Board> GetWinningBoards(List<Board> boards)
        {
            BoardValidator boardValidator = new BoardValidator();
            List<Board> winningBoards = new List<Board>();

            foreach (Board board in boards)
            {
                if (boardValidator.HasWon(board)) winningBoards.Add(board);
            }
            return winningBoards;
        }
        private static Board GetWinningBoard(List<Board> boards)
        {
            BoardValidator boardValidator = new BoardValidator();

            foreach (Board board in boards)
            {
                if (boardValidator.HasWon(board)) return board;
            }
            return null;
        }

        private static Queue<int> GetPlayQueue(IList<string> data) 
        {
            string numbers = data.First();
            Queue<int> plays = new Queue<int>();

            numbers.Split(',').Select(number => int.Parse(number)).ToList()
                .ForEach(number => plays.Enqueue(number));

            return plays;
        }

        private static List<Board> GetBoards(IList<string> data)
        {
            data = data.Skip(2).ToList();

            List<Board> boards = new List<Board>();
            
            string rawData = "";
            foreach (string item in data)
            {
                if (item != "")
                {
                    rawData += ' ' + item;
                }
                else
                {
                    boards.Add(BoardFactory.CreateBoard(rawData));
                    rawData = "";
                }
            }

            return boards;
        }

        private class BoardFactory
        {
            public static Board CreateBoard(string data)
            {
                return new Board {
                    Numbers = data.Replace("  ", " ").Trim()
                        .Split(' ').Select(number => Int32.Parse(number))
                        .ToList()
                };
            }
        }
        class Board 
        {
            public List<int> Numbers { get; set; }
            private BitArray boardState = new BitArray(25);

            public void Play(int number)
            {
                int pos = Numbers.IndexOf(number);
                if (pos != -1) boardState[pos] = true;
            }

            public int GetRemainingSum()
            {
                int score = 0;
                for (int pos = 0; pos < boardState.Length; pos++)
                {
                    if (!boardState[pos]) score += Numbers[pos];
                }
                return score;
            }

            public BitArray GetState()
            {
                return this.boardState;
            }
        }

        class BoardValidator
        {
            static readonly List<string> lineMasks = new List<string> {
                "1111100000000000000000000",
                "0000011111000000000000000",
                "0000000000111110000000000",
                "0000000000000001111100000",
                "0000000000000000000011111",
                "1000010000100001000010000",
                "0100001000010000100001000",
                "0010000100001000010000100",
                "0001000010000100001000010",
                "0000100001000010000100001"
            };

            public bool HasWon(Board board)
            {
                return HasWinningLine(board);;
            }

            private bool HasWinningLine(Board board)
            {
                foreach (String lineMask in lineMasks)
                {
                    BitArray result = MaskToBitArray(lineMask).And(board.GetState());
                    if (BitArrayToMask(result).Equals(lineMask)) return true;
                }
                return false;
            }

            private BitArray MaskToBitArray(string mask)
            {
                return new BitArray(mask.Select(c => c == '1').ToArray());
            }

            private string BitArrayToMask(BitArray array)
            {
                StringBuilder builder = new StringBuilder();
                foreach (bool bit in array.Cast<bool>()) builder.Append(bit ? "1": "0");
                return builder.ToString();
            }
        }
    }
}