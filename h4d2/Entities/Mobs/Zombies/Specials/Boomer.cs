using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Boomer : Special
{
    public Boomer(Level level, Position position) 
        : base(
            level,
            Cfg.BoomerBoundingBox,
            position,
            Cfg.Boomer,
            Cfg.BoomerHealth,
            Cfg.BoomerRunSpeed,
            Cfg.BoomerDamage,
            Cfg.BoomerColor
        )
    {
        
    }
}