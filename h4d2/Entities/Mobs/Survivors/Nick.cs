using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;

public class Nick : Survivor
{
    public Nick(Level level, Position position) 
        : base(level, position, SurvivorConfigs.Nick)
    {
        _weapon = new Deagle(level);
    }    
}