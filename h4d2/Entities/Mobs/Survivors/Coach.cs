using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Coach : Survivor
{
    public Coach(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Coach, xPosition, yPosition)
    {
        _weapon = new PumpShotgun(level, this);
    }
}