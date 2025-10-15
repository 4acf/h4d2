namespace H4D2.Infrastructure;

public sealed class RandomSingleton
{
    public static Random Instance { get; } = new();
}