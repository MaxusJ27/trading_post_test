namespace NumberGame {
    public class NumberGenerator
{

    public int GeneratedNumber { get; set; }

    public static int GenerateNumber(int minRange, int maxRange)
    {
        return new Random().Next(minRange, maxRange);
    }
    public NumberGenerator(int minRange = 1, int maxRange = 100)
    {
        GeneratedNumber = GenerateNumber(minRange, maxRange);
    }

    public int getGeneratedNumber()
    {
        return GeneratedNumber;
    }
}
}