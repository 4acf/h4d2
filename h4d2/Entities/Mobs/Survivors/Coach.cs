using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;

public class Coach : Survivor
{
    public Coach(Level level, Position position) 
        : base(level, position, SurvivorConfigs.Coach)
    {
        _weapon = new PumpShotgun(level, this);
    }
}