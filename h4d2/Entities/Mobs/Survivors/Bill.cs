using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;

public class Bill : Survivor
{
    public Bill(Level level, Position position) 
        : base(level, position, SurvivorConfigs.Bill)
    {
        _weapon = new M16(level);
    }
}