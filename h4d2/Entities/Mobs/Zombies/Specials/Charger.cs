using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Charger : Special
{
    public Charger(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.ChargerBoundingBox, Cfg.Charger, Cfg.ChargerHealth, Cfg.ChargerRunSpeed, xPosition, yPosition, Cfg.ChargerColor)
    {
        
    }
}