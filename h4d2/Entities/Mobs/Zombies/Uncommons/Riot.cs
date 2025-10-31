using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using Cfg = UncommonConfig;

public class Riot : Uncommon
{
    public Riot(Level level, Position position) 
        : base(
            level,
            position,
            Cfg.Riot,
            Cfg.RiotHealth,
            Cfg.RiotSpeed,
            Cfg.Damage,
            Cfg.RiotColor
        )
    {
        
    }
}