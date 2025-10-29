using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using Cfg = UncommonConfig;

public class Mudman : Uncommon
{
    public Mudman(Level level, int xPosition, int yPosition) 
        : base(
            level,
            Cfg.Mudman,
            Cfg.MudmanHealth,
            Cfg.MudmanSpeed,
            Cfg.Damage,
            xPosition,
            yPosition,
            Cfg.MudmanColor
        )
    {
        
    }
}