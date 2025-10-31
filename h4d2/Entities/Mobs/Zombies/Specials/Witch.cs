using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Witch : Special
{
    public Witch(Level level, Position position) 
        : base(
            level,
            Cfg.WitchBoundingBox,
            position,
            Cfg.Witch,
            Cfg.WitchHealth,
            Cfg.WitchRunSpeed,
            Cfg.WitchDamage,
            Cfg.WitchColor
        )
    {
        
    }    
}