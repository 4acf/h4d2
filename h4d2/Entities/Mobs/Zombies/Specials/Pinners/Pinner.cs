using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials.Pinners;

public abstract class Pinner : Special
{
    protected Survivor? _pinTarget;
    
    protected Pinner(Level level, Position position, SpecialConfig config)
        : base(level, position, config)
    {
        
    }

    protected override void _Die()
    {
        base._Die();
        _pinTarget?.Cleared();
    }
}