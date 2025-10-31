using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Nick : Survivor
{
    public Nick(Level level, Position position) 
        : base(level, position, Cfg.Nick, Cfg.WhiteSkinColor)
    {
        _weapon = new Deagle(level, this);
    }    
}