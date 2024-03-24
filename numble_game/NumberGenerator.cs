namespace NumberGame
{
    public class NumberGenerator
    {

        private int GeneratedNumber { get; set; }
        /*
        @param minRange: the minimum value of the range
        @param maxRange: the maximum value of the range
        /* return a random number between minRange and maxRange
        */
        public static int GenerateNumber(int minRange, int maxRange)
        {
            return new Random().Next(minRange, maxRange);
        }

        /*
        @param minRange: the minimum value of the range
        @param maxRange: the maximum value of the range
        /* Constructor that generates a random number between minRange and maxRange
        */
        public NumberGenerator(int minRange = 1, int maxRange = 100)
        {
            GeneratedNumber = GenerateNumber(minRange, maxRange);
        }
        /*
        /* return the generated number
        */
        public int getGeneratedNumber()
        {
            return GeneratedNumber;
        }
    }
}