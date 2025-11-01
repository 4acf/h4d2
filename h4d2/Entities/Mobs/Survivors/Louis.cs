using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;

public class Louis : Survivor
{
    public Louis(Level level, Position position) 
        : base(level, position, SurvivorConfigs.Louis)
    {
        _weapon = new Uzi(level, this);
    }    
}