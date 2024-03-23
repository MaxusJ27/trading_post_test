namespace NumberGame
{
    public class Player
    {
        // at least name, guess and score
        public string? Name { get; set; }
        public int[,] Guess { get; set; } = new int[5, 5];
        public int Score { get; set; }

        public Player() { }

        public Player(string name)
        {
            if (name.Length > 8 || name.Length <= 0)
            {
                throw new System.ArgumentException("Name must be at least 1 character long and at most 8 characters long.");
            }
            Name = name;
            Score = 0;
        }


        public void SetGuess(int guess, int round, int guessNumber)
        {
            Guess[round, guessNumber] = guess;
        }

        public int GetGuess(int round, int guessNumber)
        {
            return Guess[round, guessNumber];
        }
        public override string ToString()
        {
            return $"Name: {Name}, Score: {Score}";
        }
    }
}