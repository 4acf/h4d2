using H4D2.Entities.Pickups.Throwable;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Spawners;

public enum ThrowableDescriptor
{
    Molotov,
    PipeBomb,
    BileBomb
}

public static class ThrowableSpawner
{
    public static Throwable Spawn(ThrowableDescriptor descriptor, Level level, Position position)
    {
        return descriptor switch
        {
            ThrowableDescriptor.Molotov => new Molotov(level, position),
            ThrowableDescriptor.PipeBomb => new PipeBomb(level, position),
            _ => new BileBomb(level, position)
        };
    }   
}