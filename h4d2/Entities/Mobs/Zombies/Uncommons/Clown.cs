using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using Cfg = UncommonConfig;

public class Clown : Uncommon
{
    public Clown(Level level, Position position) 
        : base(
            level,
            position,
            Cfg.Clown,
            Cfg.ClownHealth,
            Cfg.ClownSpeed,
            Cfg.Damage,
            Cfg.ClownColor
        )
    {
        
    }
}