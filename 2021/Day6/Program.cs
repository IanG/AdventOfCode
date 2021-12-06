using System;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> inputs = System.IO.File.ReadAllText(args[0])
                .Split(',').Select(value => int.Parse(value)).ToList();

            List<Lanternfish> shoal = ReanimateShoal(inputs);

            Console.WriteLine($"Initial State: {String.Join(',', shoal.Select(fish => fish.GetState()).ToList())}");

            int daysOfLife = 80;

            for (int day = 1; day <= daysOfLife; day++)
            {
                List<Lanternfish> babyFish = new List<Lanternfish>();
                foreach (Lanternfish fish in shoal)
                {
                    Lanternfish baby = fish.TryAndGiveBirth();
                    if (baby != null) babyFish.Add(baby);
                }
                if (babyFish.Count > 0) shoal.AddRange(babyFish);
            }

            Console.WriteLine($"Answer 1: {shoal.Count}");

            shoal = ReanimateShoal(inputs);

            daysOfLife = 256;

            for (int day = 1; day <= daysOfLife; day++)
            {
                Console.WriteLine($"$Day {day} - {shoal.Count} fish.");
                List<Lanternfish> babyFish = new List<Lanternfish>();
                foreach (Lanternfish fish in shoal)
                {
                    Lanternfish baby = fish.TryAndGiveBirth();
                    if (baby != null) babyFish.Add(baby);
                }
                if (babyFish.Count > 0) shoal.AddRange(babyFish);
            }

            Console.WriteLine($"Answer 2: {shoal.Count}");
        }

        private static List<Lanternfish> ReanimateShoal(List<int> fishStates)
        {
            List<Lanternfish> shoal = new List<Lanternfish>();

            foreach (int fishState in fishStates)
            {
                shoal.Add(new Lanternfish(fishState));
            }

            return shoal;
        }

        class Lanternfish
        {
            private int state = 0;
            private const int GIVE_BIRTH = -1;
            private const int PREGNANT_AGAIN = 6;
            private const int NEW_BORN = 8;

            public Lanternfish()
            {
                this.state = 8;
            }

            public Lanternfish(int state)
            {
                this.state = state;
            }

            public Lanternfish TryAndGiveBirth()
            {
                //state--;

                if(--state == GIVE_BIRTH)
                {
                    state = PREGNANT_AGAIN;
                    return new Lanternfish(NEW_BORN);
                }

                return null;
            }

            public int GetState()
            {
                return state;
            }

            public override string ToString() 
            { 
                string fishType = state <= PREGNANT_AGAIN ? "🐡" : "🐟";
                return $"{fishType} State = {state}"; 
            }
        }
    }
}
