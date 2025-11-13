using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles.Emitters;

public class SpitSplatter : Emitter<VolatileSpit>
{
    public SpitSplatter(Level level, Position position)
        : base(level, position, EmitterConfigs.SpitSplatter,
            (lvl, pos, v) => new VolatileSpit(lvl, pos, v)
        )
    {
        
    }
}