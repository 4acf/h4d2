using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles.Emitters;

public class FuelSplatter : Emitter<Fuel>
{
    public FuelSplatter(Level level, Position position)
        : base(level, position, EmitterConfigs.FuelSplatter,
            (lvl, pos, v) => new Fuel(lvl, pos, v)
        )
    {
        
    }
}