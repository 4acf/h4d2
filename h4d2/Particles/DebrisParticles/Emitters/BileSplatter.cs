using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles.Emitters;

public class BileSplatter : Emitter<Bile>
{
    public BileSplatter(Level level, Position position)
        : base(level, position, EmitterConfigs.BileSplatter,
            (lvl, pos, v) => new Bile(lvl, pos, v)
        )
    {
        
    }
}