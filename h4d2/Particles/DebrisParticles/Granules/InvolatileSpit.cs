using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles.Granules;

public class InvolatileSpit : Granule
{
    public InvolatileSpit(Level level, Position position, ReadonlyVelocity parentVelocity)
        : base(level, position, GranuleConfigs.InvolatileSpit, parentVelocity)
    {
        
    }
}