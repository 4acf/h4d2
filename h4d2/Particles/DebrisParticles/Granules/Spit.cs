using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles.Granules;

public class Spit : Granule
{
    public Spit(Level level, Position position, ReadonlyVelocity parentVelocity)
        : base(level, position, GranuleConfigs.Spit, parentVelocity)
    {
        
    }
}