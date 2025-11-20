using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

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
        Bitmaps = H4D2Art.Explosion
    };
    
    public static readonly CloudletConfig Heal = new()
    {
        FrameDuration = 1.0 / 16.0,
        Bitmaps = H4D2Art.HealParticle
    };

    public static readonly CloudletConfig SmokerSmoke = new()
    {
        FrameDuration = 0.0,
        Bitmaps = H4D2Art.NullParticle
    };
}

