using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles.Granules;

public class Blood : Granule
{
    public Blood(Level level, Position position, double xv, double yv, double zv)
        : base(level, position, GranuleConfigs.Blood, xv, yv, zv)
    {
        
    }
}