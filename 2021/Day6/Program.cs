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

            long fishCount = HowManyFishAfter(256, inputs);
            Console.WriteLine($"Answer 2: {fishCount}");
        }

        private static long HowManyFishAfter(int daysOfLife, List<int> startingFish)
        {
            long[] states = new long[9];

            foreach (int fish in startingFish) states[fish]++;

            for (int day = 1; day <= daysOfLife; day++)
            {
                long newParents = states[0];
                states[0] = 0;

                for (int state = 1; state < states.Length; state++)
                {
                    states[state - 1] = states[state];
                    states[state] = 0;
                }   

                if (newParents > 0)
                {
                    states[6]+= newParents;
                    states[8]+= newParents;
                }
            }
            return states.Sum();
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
