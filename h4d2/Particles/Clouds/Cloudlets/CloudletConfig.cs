using H4D2.Infrastructure;

namespace H4D2.Particles.Clouds.Cloudlets;

public class CloudletConfig
{
    public required double FrameDuration { get; init; }
    public required Bitmap[] Bitmaps { get; init; }
}

public static class CloudletConfigs
{
    public static readonly CloudletConfig ExplosionFlame = new()
    {
        FrameDuration = 1.0 / 16.0,
        Bitmaps = Art.Explosion
    };
    
    public static readonly CloudletConfig Heal = new()
    {
        FrameDuration = 1.0 / 16.0,
        Bitmaps = Art.HealParticle
    };
}

