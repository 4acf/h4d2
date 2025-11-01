using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;

public class Zoey : Survivor
{
    public Zoey(Level level, Position position)
        : base(level, position, SurvivorConfigs.Zoey)
    {
        _weapon = new HuntingRifle(level, this);
    }
}