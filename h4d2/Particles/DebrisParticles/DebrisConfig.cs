namespace H4D2.Particles.DebrisParticles;

public class DebrisConfig
{
    public required double Drag { get; init; }
    public required double Bounce { get; init; }
    public required double MinLifetime { get; init; }
    public required double MaxLifetime { get; init; }
}

public static class DebrisConfigs
{
    public static readonly DebrisConfig Gib = new()
    {
        Drag = 0.96,
        Bounce = 0.6,
        MinLifetime = 0.9,
        MaxLifetime = 1.5
    };
}