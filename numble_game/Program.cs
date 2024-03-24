using System.Collections;
using NumberGame;
// Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 


public class Game
{
    // stores the possible range of values for comparison
    private static int MinRange = 1;
    private static int MaxRange = 100;
    private static int Attempt = 0;

    private static int AbandonNumber = 7;
    private static NumberGenerator generator = new NumberGenerator();


    private static int GeneratedNumber;

    private static Player HumanPlayer;

    private static Player ComputerPlayer;

    protected Game() { }

    public static Hashtable ScoreMap()
    {
        // map of attempts to scores
        Hashtable attemptScoreMap = new Hashtable{
            {1, 18},
            {2, 12},
            {3, 8},
            {4, 5},
            {5, 3},
            {6, 2}
        };
        return attemptScoreMap;
    }
    public static void ResetValues(int round)
    {
        Attempt = 0;
        MinRange = 0;
        MaxRange = 100;

        GeneratedNumber = NumberGenerator.GenerateNumber(MinRange, MaxRange);
        Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("\t\t║                                                                                  ║");
        Console.WriteLine($"\t\t║                                  Starting Round {round}                                ║");
        Console.WriteLine("\t\t║                                                                                  ║");
        Console.WriteLine("\t\t╚══════════════════════════════════════════════════════════════════════════════════╝");
    }
    public static void DisplayWelcomeMessage()
    {
        Console.WriteLine("\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("\t\t║                              Welcome to the Numble Game!                         ║");
        Console.WriteLine("\t\t║                     Your mission, if you choose to accept it, is to guess        ║");
        Console.WriteLine("\t\t║                          the secret number, with the following rules:            ║");
        Console.WriteLine("\t\t║                                                                                  ║");
        Console.WriteLine("\t\t║ Players take turns guessing a number between 1 and 100.                          ║");
        Console.WriteLine("\t\t║ If a guess is wrong, players are told if the hidden number is higher or lower.   ║");
        Console.WriteLine("\t\t║ Guesses must be within the current possible range.                               ║");
        Console.WriteLine("\t\t║ Entering numbers outside 1 - 100 prompts a warning.                              ║");
        Console.WriteLine("\t\t║ Non-numeric input also prompts a warning.                                        ║");
        Console.WriteLine("\t\t║ Correct guesses end the round, awarding points based on attempts.                ║");
        Console.WriteLine("\t\t║ Entering 999 abandons the round or computer may abandon randomly.                ║");
        Console.WriteLine("\t\t║ If no player guessed correctly, then the closest one scores 1 point.             ║");
        Console.WriteLine("\t\t║                                                                                  ║");
        Console.WriteLine("\t\t╚══════════════════════════════════════════════════════════════════════════════════╝");

    }

    public static void RequestPlayerName()
    {

        string playerName;
        do
        {
            Console.WriteLine("\n\nPlease enter your name (within 8 characters): ");

            playerName = Console.ReadLine();

            if (playerName?.Length > 8 || playerName?.Length <= 0)
            {
                Console.WriteLine("Name must be at least 1 character long and at most 8 characters long.");
            }
            else
            {
                HumanPlayer = new Player(playerName);
                ComputerPlayer = new Player("Computer");
                break;
            }

        } while (playerName?.Length > 8 || playerName?.Length <= 0);
        Console.WriteLine($"\n{HumanPlayer.ToString()}");
    }

    public static bool CompareNumber(int guess)
    {
        if (guess == GeneratedNumber)
        {
            return true;
        }
        return false;
    }

    public static void RequestPlayerGuess(int round, int guessNumber)
    {
        // end the round when the player decides to abandon the round
        Console.WriteLine($"\n\nPlease enter your guess for round {round}, guess {guessNumber}: ");
        string input = Console.ReadLine();
        while (input.Any(char.IsLetter) || input == null || input == "" || (int.Parse(input) > 100 && int.Parse(input) != 999) || int.Parse(input) < 1)
        {
            Console.WriteLine("Guess must be non-empty and a number between 1 and 100.");
            Console.WriteLine("Please re-enter your guess: ");
            input = Console.ReadLine();
        }
        int guess = input != null ? int.Parse(input) : 0;
        // case: number less than 1 or greater than 100
        if (guess <= MinRange || (guess >= MaxRange && guess != 999))
        {
            Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"\t\t║                      {string.Format("{0,-60}", $"Guess must be within the range of {MinRange} and {MaxRange}.")}║");
            Console.WriteLine("\t\t╚══════════════════════════════════════════════════════════════════════════════════╝");
        }

        else
        {
            SetRange(guess);
            HumanPlayer.SetGuess(guess, round, guessNumber);
            Attempt += 1;
        }
        int abandonIndicator = NumberGenerator.GenerateNumber(1, 20);
        // if number is 7, computer abandons the round with (1/20) probability
        if (abandonIndicator == AbandonNumber)
        {
            ComputerPlayer.SetGuess(999, round, guessNumber);
        }
        else
        {
            int computerGuess = NumberGenerator.GenerateNumber(MinRange, MaxRange);
            ComputerPlayer.SetGuess(computerGuess, round, guessNumber);
            Attempt += 1;
            DisplayGuessResult(round, guessNumber);
        }
    }

    public static void DisplayGuessResult(int round, int guessNumber)
    {
        string humanGuess;
        int guess = HumanPlayer.GetGuess(round, guessNumber);
        if (guess == -99)
        {
            humanGuess = "N/A";
        }
        else
        {
            humanGuess = guess.ToString();
        }
        // handle shifting of bounding boxes when different number of digits
        Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"\t\t║                                {string.Format("{0,-50}", $"    Guess {guessNumber} Result:")}║");
        Console.WriteLine($"\t\t║                                  {string.Format("{0,-48}", $"Player's guess: {humanGuess}")}║");
        Console.WriteLine($"\t\t║                                  {string.Format("{0,-48}", $"Computer's guess: {ComputerPlayer.GetGuess(round, guessNumber)}")}║");
        Console.WriteLine("\t\t╚══════════════════════════════════════════════════════════════════════════════════╝");


    }

    public static bool CheckGuess(int round, int guessNumber)
    {

        if (HumanPlayer.GetGuess(round, guessNumber) == 999)
        {
            Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"\t\t║                     {string.Format("{0,-60}", $"You have decided to abandon this round.")} ║");
            ComputerPlayer.Score += (int)ScoreMap()[Attempt - 1];
            return true;
        }


        else if (ComputerPlayer.GetGuess(round, guessNumber) == 999)
        {
            Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"\t\t║                     {string.Format("{0,-60}", $"The computer has decided to abandon this round.")} ║");
            HumanPlayer.Score += (int)ScoreMap()[Attempt];
            return true;
        }


        else if (CompareNumber(HumanPlayer.GetGuess(round, guessNumber)))
        {
            Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"\t\t║                     {string.Format("{0,-60}", $"You guessed the number correctly for round {round}!")} ║");
            HumanPlayer.Score += (int)ScoreMap()[Attempt];
            return true;
        }
        else if (CompareNumber(ComputerPlayer.GetGuess(round, guessNumber)))
        {
            Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"\t\t║           {string.Format("{0,-64}", $"Oops! The computer guessed the number correctly! You lost round {round}!")}     ║");
            ComputerPlayer.Score += (int)ScoreMap()[Attempt];
            return true;
        }

        if (guessNumber == 3)
        {
            int humanDistance = Math.Abs(HumanPlayer.GetGuess(round, guessNumber) - GeneratedNumber);
            int computerDistance = Math.Abs(ComputerPlayer.GetGuess(round, guessNumber) - GeneratedNumber);
            if (humanDistance < computerDistance)
            {
                Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"\t\t║         {string.Format("{0,-72}", $"Your answer was closer to the secret number, you won round {round}!")} ║");
                HumanPlayer.Score += 1;
            }
            else if (humanDistance > computerDistance)
            {
                ComputerPlayer.Score += 1;
                Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"\t\t║{string.Format("{0,-52}", $"  Oops! The computer's answer was closer to the secret number, you lost round {round}!")}  ║");
            }
            else
            {
                Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"\t\t║                              {string.Format("{0,-52}", $"It's a draw for round {round}!")}║");
            }
            return true;
        }
        return false;
    }

    public static void DisplayRoundScore(int round)
    {
        Console.WriteLine($"\t\t║                              {string.Format("{0,-52}", $"    Round {round} score:")}║");
        Console.WriteLine($"\t\t║                                {string.Format("{0,-50}", $"Secret Number: {GeneratedNumber}")}║");
        Console.WriteLine($"\t\t║                              {string.Format("{0,-52}", $"Player's Current Score: {HumanPlayer.Score}")}║");
        Console.WriteLine($"\t\t║                            {string.Format("{0,-54}", $"Computer's Current Score: {ComputerPlayer.Score}")}║");
        Console.WriteLine("\t\t╚══════════════════════════════════════════════════════════════════════════════════╝");
    }

    public static void SetRange(int guess)
    {
        Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"\t\t║                                        Hint:                                     ║");
        if (guess > GeneratedNumber && guess != 999)
        {
            MaxRange = guess;
            Console.WriteLine($"\t\t║                    {string.Format("{0,-62}", $"    The guess should be lower than {MaxRange}")}║");
        }
        else if (guess < GeneratedNumber)
        {
            MinRange = guess;
            Console.WriteLine($"\t\t║                    {string.Format("{0,-62}", $"    The guess should be higher than {MinRange}")}║");
        }
        Console.WriteLine("\t\t╚══════════════════════════════════════════════════════════════════════════════════╝");
    }


    public static void DisplayTotalScore()
    {
        Console.WriteLine("\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"\t\t║                                        Game                                      ║");
        Console.WriteLine($"\t\t║                          {string.Format("{0,-56}", $"Your Score: {HumanPlayer.Score}, Computer Score: {ComputerPlayer.Score}")}║");
        if (HumanPlayer.Score > ComputerPlayer.Score)
        {
        Console.WriteLine($"\t\t║                              {string.Format("{0,-52}", $"Congratulations, you WON!")}║");
        }
        else if (HumanPlayer.Score < ComputerPlayer.Score)
        {
        Console.WriteLine($"\t\t║                                {string.Format("{0,-50}", $"Oops, you LOST the game!")}║");
        }
        else
        {
        Console.WriteLine($"\t\t║                                    {string.Format("{0,-46}", $"It's a DRAW")}║");
        }
        Console.WriteLine("\t\t╚══════════════════════════════════════════════════════════════════════════════════╝");
    }

    public static void Main(string[] args)
    {
        DisplayWelcomeMessage();
        RequestPlayerName();

        for (int i = 1; i <= 4; i++)
        {
            ResetValues(i);
            bool guessResult = false;
            for (int j = 1; j <= 3; j++)
            {
                if (guessResult)
                {
                    break;
                }
                else
                {
                    RequestPlayerGuess(i, j);
                }
                guessResult = CheckGuess(i, j);

            }
            DisplayRoundScore(i);
        }
        DisplayTotalScore();
    }
}
