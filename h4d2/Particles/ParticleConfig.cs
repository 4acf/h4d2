namespace H4D2.Particles;

public static class ParticleConfig
{
    public const double BaseFramerate = 60;
    
    public const double BloodDrag = 0.96;
    public const double BloodBounce = 0.1;

    public const double BloodSplatterDrag = 0.98;
    public const double BloodSplatterBounce = 0.6;
    public const double BloodSplatterLifetimeScale = 0.25;
    
    public const double DeathSplatterDrag = 0.96;
    public const double DeathSplatterBounce = 0.6;

    public const double MinDebrisLifetime = 0.6;
    public const double MaxDebrisLifetime = 1.0;

    public const double ExplosionLifetime = 0.5;
    
    public const double MinSmokeLifetime = 0.1;
    public const double MaxSmokeLifetime = 0.3;
    public const double MinSmokeOpacity = 0;
    public const double MaxSmokeOpacity = 0.5;
}

