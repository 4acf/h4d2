using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using Cfg = UncommonConfig;

public class Hazmat : Uncommon
{
    public Hazmat(Level level, Position position) 
        : base(
            level,
            position,
            Cfg.Hazmat,
            Cfg.HazmatHealth,
            Cfg.HazmatSpeed,
            Cfg.Damage,
            Cfg.HazmatColor
        )
    {
        
    }
}