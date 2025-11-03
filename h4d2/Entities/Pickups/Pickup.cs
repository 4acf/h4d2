using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups;

public abstract class Pickup : Entity
{
    protected readonly int _pickupType;
    private const double _positionChangeSeconds = 1.0;
    private double _positionChangeSecondsLeft;
    private bool _isAtStartingPosition;
    
    protected Pickup(Level level, Position position, PickupConfig config)
        : base(level, position, config.BoundingBox)
    {
        _pickupType = config.PickupType;
        _positionChangeSecondsLeft = _positionChangeSeconds;
        _isAtStartingPosition = true;
    }

    public override void Update(double elapsedTime)
    {
        _positionChangeSecondsLeft -= elapsedTime;
        if (_positionChangeSecondsLeft <= 0)
        {
            _positionChangeSecondsLeft = _positionChangeSeconds;
            _position.Z += 1 * (_isAtStartingPosition ? 1 : -1);
            _isAtStartingPosition = !_isAtStartingPosition;
        }
    }

    public virtual void PickUp(Survivor survivor)
    {
        Removed = true;
    }
}