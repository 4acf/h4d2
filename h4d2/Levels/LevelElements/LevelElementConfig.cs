namespace H4D2.Levels.LevelElements;

public class LevelElementConfig
{
    public required int Type { get; init; }
}

public static class LevelElementConfigs
{
    public static readonly LevelElementConfig Wall = new()
    {
        Type = 0
    };

    public static readonly LevelElementConfig ZombieWall = new()
    {
        Type = 1
    };

    public static readonly LevelElementConfig EdgeWall = new()
    {
        Type = 2
    };
}