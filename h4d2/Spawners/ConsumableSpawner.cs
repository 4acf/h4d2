using H4D2.Entities.Pickups.Consumables;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Spawners;

public enum ConsumableDescriptor
{
    FirstAidKit,
    Pills,
    Adrenaline
}

public static class ConsumableSpawner
{
    public static Consumable Spawn(ConsumableDescriptor descriptor, Level level, Position position)
    {
        return descriptor switch
        {
            ConsumableDescriptor.FirstAidKit => new FirstAidKit(level, position),
            ConsumableDescriptor.Pills => new Pills(level, position),
            _ => new Adrenaline(level, position)
        };
    }   
}