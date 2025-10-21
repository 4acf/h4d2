using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Ellis : Survivor
{
    public Ellis(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Ellis, xPosition, yPosition)
    {
        _weapon = new GrenadeLauncher(level, this);
    }
}