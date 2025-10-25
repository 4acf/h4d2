using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Hunter : Special
{
    public Hunter(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.HunterBoundingBox, Cfg.Hunter, Cfg.HunterHealth, Cfg.HunterRunSpeed, xPosition, yPosition, Cfg.HunterColor)
    {
        
    }    
}