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

    public void TankCleared()
    {
        if (this is Smoker)
            _StopPinning();
        else
            _Die();
    }

    protected override void _UpdateTarget()
    {
        List<Survivor> survivors = _level.GetEntities<Survivor>();
        int numPinned = survivors.Count(survivor => survivor.IsPinned);
        _target = survivors.Count == numPinned && numPinned > 0 ? 
            survivors[0] :
            _level.GetNearestUnpinnedSurvivor(Position);
    }

    protected virtual void _StopPinning()
    {
        _pinTarget = null;
    }
    
    protected override void _Die()
    {
        base._Die();
        _pinTarget?.Cleared();
    }
}