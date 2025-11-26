using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles.Granules;

public class InvolatileBile : Granule
{
    public InvolatileBile(Level level, Position position, ReadonlyVelocity velocity)
        : base(level, position, GranuleConfigs.InvolatileBile, velocity)
    {
        
    }
}