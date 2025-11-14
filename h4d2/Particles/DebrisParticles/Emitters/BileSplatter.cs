using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles.Emitters;

public class BileSplatter : Emitter<VolatileBile>
{
    public BileSplatter(Level level, Position position)
        : base(level, position, EmitterConfigs.BileSplatter,
            (lvl, pos, v) => new VolatileBile(lvl, pos, v)
        )
    {
        
    }
}