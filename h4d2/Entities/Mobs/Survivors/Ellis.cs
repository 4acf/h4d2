using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;


public class Ellis : Survivor
{
    public Ellis(Level level, Position position) 
        : base(level, position, SurvivorConfigs.Ellis)
    {
        _weapon = new GrenadeLauncher(level);
    }
}