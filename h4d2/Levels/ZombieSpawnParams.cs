namespace H4D2.Levels;

public record ZombieSpawnParams(
    int MinZombiesAlive,
    int MaxZombiesAlive,
    int MinSpawnWaveSize,
    int MaxSpawnWaveSize
);