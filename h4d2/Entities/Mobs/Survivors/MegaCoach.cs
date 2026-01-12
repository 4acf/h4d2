using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;

public class MegaCoach : Survivor
{
    public MegaCoach(Level level, Position position)
        : base(level, position, SurvivorConfigs.MegaCoach)
    {
        _weapon = new MegaCoachShotgun(level);
    }
}