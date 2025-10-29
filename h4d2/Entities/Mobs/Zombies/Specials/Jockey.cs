using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using Cfg = SpecialConfig;

public class Jockey : Special
{
    public Jockey(Level level, int xPosition, int yPosition) 
        : base(
            level,
            Cfg.JockeyBoundingBox,
            Cfg.Jockey,
            Cfg.JockeyHealth,
            Cfg.JockeyRunSpeed,
            Cfg.JockeyDamage,
            xPosition,
            yPosition,
            Cfg.JockeyColor
        )
    {
        
    }    
}