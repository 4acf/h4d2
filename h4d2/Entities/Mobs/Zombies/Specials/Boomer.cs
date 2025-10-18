using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Boomer : Special
{
    public Boomer(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.BoomerBoundingBox, Cfg.Boomer, Cfg.BoomerHealth, Cfg.BoomerRunSpeed, xPosition, yPosition)
    {
        
    }
}