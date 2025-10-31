using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Hunter : Special
{
    public Hunter(Level level, Position position) 
        : base(
            level,
            Cfg.HunterBoundingBox,
            position,
            Cfg.Hunter,
            Cfg.HunterHealth,
            Cfg.HunterRunSpeed,
            Cfg.HunterDamage,
            Cfg.HunterColor
        )
    {
        
    }    
}