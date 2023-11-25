const int ROCK_SCORE = 1;
const int PAPER_SCORE = 2;
const int SCISSORS_SCORE = 3;
const int WIN_SCORE = 6;
const int DRAW_SCORE = 3;
const int LOSE_SCORE = 0;

IEnumerable<string> lines = System.IO.File.ReadLines(args[0]);

// Part 1
Dictionary<string, int> initialStrategy = new ()
{
    { "A X", DRAW_SCORE + ROCK_SCORE },     // ROCK vs ROCK = DRAW 4 (3+1)
    { "A Y", WIN_SCORE + PAPER_SCORE },     // ROCK vs PAPER = WIN 8 (6+2) 
    { "A Z", LOSE_SCORE + SCISSORS_SCORE }, // ROCK vs SCISSORS = LOSE 3 (3+0)
    { "B X", LOSE_SCORE + ROCK_SCORE },     // PAPER vs ROCK = LOSE (1+0)
    { "B Y", DRAW_SCORE + PAPER_SCORE },    // PAPER vs PAPER = DRAW 5 (3+2)
    { "B Z", WIN_SCORE + SCISSORS_SCORE },  // PAPER vs SCISSORS = WIN 9 (6+3)
    { "C X", WIN_SCORE + ROCK_SCORE },      // SCISSORS vs ROCK = WIN 7 (6+1)
    { "C Y", LOSE_SCORE + PAPER_SCORE },    // SCISSORS vs PAPER = LOSE 2 (2+0)
    { "C Z", DRAW_SCORE + SCISSORS_SCORE }  // SCISSOR vs SCISSORS = DRAW 6 (3+3) 
};

int gameScore = 0;
foreach (string line in lines)
{
    gameScore += initialStrategy[line];
}

Console.WriteLine($"Part 1: {gameScore}");

// Part 2 
Dictionary<string, int> actualStrategy = new ()
{
    { "A X", LOSE_SCORE + SCISSORS_SCORE },        
    { "B X", LOSE_SCORE + ROCK_SCORE },            
    { "C X", LOSE_SCORE + PAPER_SCORE },
    { "A Y", DRAW_SCORE + ROCK_SCORE },
    { "B Y", DRAW_SCORE + PAPER_SCORE },
    { "C Y", DRAW_SCORE + SCISSORS_SCORE },
    { "A Z", WIN_SCORE + PAPER_SCORE },
    { "B Z", WIN_SCORE + SCISSORS_SCORE },
    { "C Z", WIN_SCORE + ROCK_SCORE } 
};

gameScore = 0;
foreach(string line in lines)
{
    gameScore += actualStrategy[line];
}

Console.WriteLine($"Part 2: {gameScore}");