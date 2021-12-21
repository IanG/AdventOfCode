using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(GetPlayers(args[0]), new DeterministicDice(), 1000);

            while(game.Winner == null)
            {
                game.TakeTurns();
            }

            Console.WriteLine($"Answer 1 {game.Loser.Score * game.Dice.RollCount}");
        }

        public static List<Player> GetPlayers(string inputFile)
        {
            const string PLAYER_REGEX = @"^Player (\d*) starting position: (\d*)$";

            List<Player> players = new List<Player>();
            
            foreach (string line in File.ReadLines(inputFile))
            {
                MatchCollection matches = Regex.Matches(line, PLAYER_REGEX);

                if (matches.Count > 0 && matches[0].Groups.Count > 0)
                {
                    GroupCollection groups = matches[0].Groups;
                    players.Add(new Player(groups[1].Value, int.Parse(groups[2].Value)));
                }
            }

            return players;
        }
    }

    class Game
    {
        private readonly DeterministicDice dice;
        private readonly List<Player> players;
        private readonly int winningScore;
        private Player winner;
        private Player loser;

        public DeterministicDice Dice { get { return dice; } }
        public Player Winner { get { return winner; } }
        public Player Loser { get { return loser; } }

        public Game(List<Player> players, DeterministicDice dice, int winningScore)
        {
            this.players = players;
            this.dice = dice;
            this.winningScore = winningScore;
        }

        public void TakeTurns()
        {
            foreach (Player player in players)
            {
                player.TakeTurn(dice);
                if (player.Score >= winningScore) 
                {
                    winner = player;
                    loser = players.Where(p => !p.Equals(winner)).First();
                    break;
                }
            }
        }
    }

    class Player
    {
        private readonly int MAX_POSITION = 10;
        public string Name { get; }
        public int Score { get; private set; } 
        public int Position { get; private set; }
        
        public Player(string name, int startPosition)
        {
            Name = name;
            Position = startPosition;
            Score = 0;
        }

        public void TakeTurn(DeterministicDice dice)
        {            
            int newPosition = Position + dice.Roll() + dice.Roll() + dice.Roll();

            if (newPosition > MAX_POSITION) 
            {
                newPosition = newPosition % MAX_POSITION;
                if (newPosition == 0) newPosition = MAX_POSITION;
            }

            Score += Position = newPosition;
        }

        public override string ToString()
        {
            return $"Name={Name}, Position={Position}, Score={Score}";
        }

        public override bool Equals(Object obj) 
        {
            if (obj == null) return false;
            if (!(obj is Player)) return false;

            Player other = (Player)obj;

            return Name == other.Name;
        }

        public override int GetHashCode() 
        {
            return Name.GetHashCode();
        }

    }
    class DeterministicDice
    {
        private readonly int MAX_NUMBER = 100;
        private int rollCount = 0;
        private int nextNumber = 1;
        public int RollCount { get { return rollCount; } }

        public int Roll()
        {
            rollCount++;
            if (nextNumber > MAX_NUMBER) nextNumber = 1;
            return nextNumber++;
        }

        public override string ToString()
        {
            return $"RollCount={RollCount}, NextNumber={nextNumber}";
        }
    }
}
