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
    public static readonly DebrisConfig Bile = new()
    {
        Drag = 0.96,
        Bounce = 0.1,
        MinLifetime = 20.0,
        MaxLifetime = 20.0
    };

    public static readonly DebrisConfig BileSplatter = new()
    {
        Drag = 0.95,
        Bounce = 0.6,
        MinLifetime = 0.6,
        MaxLifetime = 1.0
    };

    public static readonly DebrisConfig Blood = new()
    {
        Drag = 0.96,
        Bounce = 0.1,
        MinLifetime = 0.6,
        MaxLifetime = 1.0
    };

    public static readonly DebrisConfig BloodSplatter = new()
    {
        Drag = 0.98,
        Bounce = 0.6,
        MinLifetime = 0.15,
        MaxLifetime = 0.25
    };

    public static readonly DebrisConfig Fuel = new()
    {
        Drag = 0.98,
        Bounce = 0.0,
        MinLifetime = 20.0,
        MaxLifetime = 20.0
    };

    public static readonly DebrisConfig FuelSplatter = new()
    {
        Drag = 0.98,
        Bounce = 0.6,
        MinLifetime = 0.6,
        MaxLifetime = 1.0
    };

    public static readonly DebrisConfig Gib = new()
    {
        Drag = 0.96,
        Bounce = 0.6,
        MinLifetime = 0.9,
        MaxLifetime = 1.5
    };
}