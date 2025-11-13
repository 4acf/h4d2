namespace H4D2.Particles.DebrisParticles.Granules;

public class GranuleConfig : DebrisConfig
{
    public required int Color { get; init; }
}

public static class GranuleConfigs
{
    public static readonly GranuleConfig Bile = new()
    {
        Drag = 0.96,
        Bounce = 0.1,
        MinLifetime = 20.0,
        MaxLifetime = 20.0,
        Color = 0x5a6e38
    };
    
    public static readonly GranuleConfig Blood = new()
    {
        Drag = 0.96,
        Bounce = 0.1,
        MinLifetime = 0.6,
        MaxLifetime = 1.0,
        Color = 0xa00000
    };
    
    public static readonly GranuleConfig Fuel = new()
    {
        Drag = 0.98,
        Bounce = 0.0,
        MinLifetime = 20.0,
        MaxLifetime = 20.0,
        Color = 0x4d4c47
    };

    public static readonly GranuleConfig Spit = new()
    {
        Drag = 0.98,
        Bounce = 0.0,
        MinLifetime = 0.6,
        MaxLifetime = 1.0,
        Color = 0x9fff05,
    };
}