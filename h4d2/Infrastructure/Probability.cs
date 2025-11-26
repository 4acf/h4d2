namespace H4D2.Infrastructure;

public static class Probability
{
    private static readonly Random _random = RandomSingleton.Instance;

    public static bool OneIn(uint outcomes) => _random.Next((int)outcomes) == 0;

    public static bool Percent(uint percentage) => _random.Next(100) < percentage;
}