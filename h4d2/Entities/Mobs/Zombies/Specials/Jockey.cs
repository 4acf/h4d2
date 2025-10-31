using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Jockey : Special
{
    public Jockey(Level level, Position position) 
        : base(
            level,
            Cfg.JockeyBoundingBox,
            position,
            Cfg.Jockey,
            Cfg.JockeyHealth,
            Cfg.JockeyRunSpeed,
            Cfg.JockeyDamage,
            Cfg.JockeyColor
        )
    {
        
    }    
}