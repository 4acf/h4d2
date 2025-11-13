namespace H4D2.Particles.DebrisParticles.Emitters;

public class EmitterConfig : DebrisConfig;

public static class EmitterConfigs
{
    public static readonly EmitterConfig BileSplatter = new()
    {
        Drag = 0.95,
        Bounce = 0.6,
        MinLifetime = 0.6,
        MaxLifetime = 1.0
    };

    public static readonly EmitterConfig BloodSplatter = new()
    {
        Drag = 0.98,
        Bounce = 0.6,
        MinLifetime = 0.15,
        MaxLifetime = 0.25
    };

    public static readonly EmitterConfig FuelSplatter = new()
    {
        Drag = 0.98,
        Bounce = 0.6,
        MinLifetime = 0.6,
        MaxLifetime = 1.0
    };

    public static readonly EmitterConfig SpitSplatter = new()
    {
        Drag = 0.98,
        Bounce = 0.3,
        MinLifetime = 0.8,
        MaxLifetime = 1.2
    };
}