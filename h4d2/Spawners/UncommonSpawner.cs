using H4D2.Entities.Mobs.Zombies.Uncommons;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Spawners;

public enum UncommonDescriptor
{
    Hazmat,
    Clown,
    Mudman,
    Worker,
    Riot
}

public static class UncommonSpawner
{
    public static Uncommon Spawn(UncommonDescriptor descriptor, Level level, Position position)
    {
        return descriptor switch
        {
            UncommonDescriptor.Hazmat => new Hazmat(level, position),
            UncommonDescriptor.Clown => new Clown(level, position),
            UncommonDescriptor.Mudman => new Mudman(level, position),
            UncommonDescriptor.Worker => new Worker(level, position),
            _ => new Riot(level, position)
        };
    }   
}