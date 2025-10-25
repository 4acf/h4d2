using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Smoker : Special
{
    public Smoker(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.SmokerBoundingBox, Cfg.Smoker, Cfg.SmokerHealth, Cfg.SmokerRunSpeed, xPosition, yPosition, Cfg.SmokerColor)
    {
        
    }
}