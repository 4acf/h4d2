using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Spitter : Special
{
    public Spitter(Level level, Position position) 
        : base(
            level,
            Cfg.SpitterBoundingBox,
            position,
            Cfg.Spitter,
            Cfg.SpitterHealth,
            Cfg.SpitterRunSpeed,
            Cfg.SpitterDamage,
            Cfg.SpitterColor
        )
    {
        
    }    
}