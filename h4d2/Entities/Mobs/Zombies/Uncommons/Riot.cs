using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using Cfg = UncommonConfig;

public class Riot : Uncommon
{
    public Riot(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Riot, Cfg.RiotHealth, Cfg.RiotSpeed, xPosition, yPosition, Cfg.RiotColor)
    {
        
    }
}