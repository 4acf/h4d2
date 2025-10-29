using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Spitter : Special
{
    public Spitter(Level level, int xPosition, int yPosition) 
        : base(
            level,
            Cfg.SpitterBoundingBox,
            Cfg.Spitter,
            Cfg.SpitterHealth,
            Cfg.SpitterRunSpeed,
            Cfg.SpitterDamage,
            xPosition,
            yPosition,
            Cfg.SpitterColor
        )
    {
        
    }    
}