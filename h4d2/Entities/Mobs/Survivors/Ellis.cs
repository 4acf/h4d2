using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Ellis : Survivor
{
    public Ellis(Level level, Position position) 
        : base(level, position, Cfg.Ellis, Cfg.WhiteSkinColor)
    {
        _weapon = new GrenadeLauncher(level, this);
    }
}