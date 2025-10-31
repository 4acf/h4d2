using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Charger : Special
{
    public Charger(Level level, Position position) 
        : base(
            level,
            Cfg.ChargerBoundingBox,
            position,
            Cfg.Charger,
            Cfg.ChargerHealth,
            Cfg.ChargerRunSpeed,
            Cfg.ChargerDamage,
            Cfg.ChargerColor
        )
    {
        
    }
}