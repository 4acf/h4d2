using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Smoker : Special
{
    public Smoker(Level level, Position position) 
        : base(
            level,
            Cfg.SmokerBoundingBox,
            position,
            Cfg.Smoker,
            Cfg.SmokerHealth,
            Cfg.SmokerRunSpeed,
            Cfg.SmokerDamage,
            Cfg.SmokerColor
        )
    {
        
    }
}