using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Tank : Special
{
    public Tank(Level level, Position position) 
        : base(
            level,
            Cfg.TankBoundingBox,
            position,
            Cfg.Tank,
            Cfg.TankHealth,
            Cfg.TankRunSpeed,
            Cfg.TankDamage,
            Cfg.TankColor
        )
    {
        
    }
}