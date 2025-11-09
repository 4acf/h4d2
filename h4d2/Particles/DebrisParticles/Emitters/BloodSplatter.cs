using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Particles.DebrisParticles.Emitters;

public class BloodSplatter : Emitter<Blood>
{
    public BloodSplatter(Level level, Position position)
        : base(level, position, EmitterConfigs.BloodSplatter, 
             (lvl, pos, v) => new Blood(lvl, pos, v)   
        )
    {
        
    }
}