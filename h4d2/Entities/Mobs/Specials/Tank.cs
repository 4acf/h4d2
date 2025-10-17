using H4D2.Levels;

namespace H4D2.Entities.Mobs.Specials;
using Cfg = SpecialConfig;

public class Tank : Special
{
    public Tank(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.TankBoundingBox, Cfg.Tank, Cfg.TankHealth, Cfg.TankRunSpeed, xPosition, yPosition)
    {
        
    }
}