namespace H4D2.Particles.Clouds;

public class CloudConfig
{
    public required double ParticleMultiplier { get; init; }
    public required double Lifetime { get; init; }
}

public static class CloudConfigs
{
    public static readonly CloudConfig Explosion = new()
    {
        ParticleMultiplier = 10.0,
        Lifetime = 0.5
    };

    public static readonly CloudConfig Heal = new()
    {
        ParticleMultiplier = 2.0,
        Lifetime = 0.5
    };

    public static readonly CloudConfig SmokerSmoke = new()
    {
        ParticleMultiplier = 5.0,
        Lifetime = 0.75
    };
}