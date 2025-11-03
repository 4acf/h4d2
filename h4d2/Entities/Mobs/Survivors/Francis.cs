using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;


public class Francis : Survivor
{
    public Francis(Level level, Position position) 
        : base(level, position, SurvivorConfigs.Francis)
    {
        _weapon = new AutoShotgun(level);
    }    
}