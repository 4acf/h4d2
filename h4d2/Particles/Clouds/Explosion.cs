using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Clouds.Cloudlets;

namespace H4D2.Particles.Clouds;

public class Explosion : Cloud<ExplosionFlame>
{
    public Explosion(Level level, Position position, double splashRadius)
        : base(
            level,
            position,
            CloudConfigs.Explosion,
            splashRadius,
            (lvl, pos) => new ExplosionFlame(lvl, pos)   
        )
    {
        
    }
}