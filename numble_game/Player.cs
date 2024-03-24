namespace NumberGame
{
    public class Player
    {
        // at least name, guess and score
        private string? Name { get; set; }
        private int[,] Guess { get; set; } = new int[5, 5];
        private int Score { get; set; }

        public Player() { }

        /*
        @param name: the name of the player
        /* Constructor that sets the name of the player and initializes the score to 0
        */
        public Player(string name)
        {
            if (name.Length > 8 || name.Length <= 0)
            {
                throw new ArgumentException("Name must be at least 1 character long and at most 8 characters long.");
            }
            Name = name;
            Score = 0;
        }

        /*
        @param guess: player's guess
        @param round: the current number of rounds
        @param guessNumber: the current number of guesses
        /* Set the player's guess for the current round and guess number
        */
        public void SetGuess(int guess, int round, int guessNumber)
        {
            Guess[round, guessNumber] = guess;
        }
        /*
        @param round: the current number of rounds
        @param guessNumber: the current number of guesses
        /* Get the player's guess for the current round and guess number
        /* If the guess is 0, then player has made a guess outside of range, return -99 
        /* so that the value will have a higher distance from the generated number
        */
        public int GetGuess(int round, int guessNumber)
        {
            if (Guess[round, guessNumber] == 0) {
                return -99;
            }
            return Guess[round, guessNumber];
        }

        /*
        /* return the player's score
        */
        public int GetScore() {
            return Score;
        }
        /*
        /* Set the player's score
        */
        public void SetScore(int score) {
            Score = score;
        }

        /*
        Add the player's score
        */
        public void AddScore(int score) {
            Score += score;
        }
        /*
        Return the player's information in a formatted string
        */
        public override string ToString()
        {
            return $"\n\n\t\t╔══════════════════════════════════════════════════════════════════════════════════╗" +
       $"\n\t\t║                                    Player                                        ║" +
       $"\n\t\t║                              {string.Format("{0,-52}", $"    Name: {Name}")}║" +
       $"\n\t\t║                              {string.Format("{0,-52}", $"    Score: {Score}")}║" +
       $"\n\t\t╚══════════════════════════════════════════════════════════════════════════════════╝";

        }
    }
}