using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Coach : Survivor
{
    public Coach(Level level, Position position) 
        : base(level, position, Cfg.Coach, Cfg.BlackSkinColor)
    {
        _weapon = new PumpShotgun(level, this);
    }
}