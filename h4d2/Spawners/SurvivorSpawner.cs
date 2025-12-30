using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Spawners;

public enum SurvivorDescriptor
{
    Coach,
    Nick,
    Ellis,
    Rochelle,
    Bill,
    Francis,
    Louis,
    Zoey
}

public static class SurvivorSpawner
{
    public static Survivor Spawn(SurvivorDescriptor descriptor, Level level, Position position)
    {
        return descriptor switch
        {
            SurvivorDescriptor.Coach => new Coach(level, position),
            SurvivorDescriptor.Nick => new Nick(level, position),
            SurvivorDescriptor.Ellis => new Ellis(level, position),
            SurvivorDescriptor.Rochelle => new Rochelle(level, position),
            SurvivorDescriptor.Bill => new Bill(level, position),
            SurvivorDescriptor.Francis => new Francis(level, position),
            SurvivorDescriptor.Louis => new Louis(level, position),
            _ => new Zoey(level, position)
        };
    }
}