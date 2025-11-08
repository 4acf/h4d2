using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles.Emitters;

public class BloodSplatter : Emitter<Blood>
{
    public BloodSplatter(Level level, Position position)
        : base(level, position, EmitterConfigs.BloodSplatter, 
             (lvl, pos, xv, yv, zv) => new Blood(lvl, pos, xv, yv, zv)   
        )
    {
        
    }
}