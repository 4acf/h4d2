namespace H4D2.Infrastructure;

public static class RandomSingleton
{
    public static Random Instance { get; } = new();
}