using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;
using Cfg = SpecialConfig;

public class Witch : Special
{
    public Witch(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.WitchBoundingBox, Cfg.Witch, Cfg.WitchHealth, Cfg.WitchRunSpeed, xPosition, yPosition)
    {
        
    }    
}