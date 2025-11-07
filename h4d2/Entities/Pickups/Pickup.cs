using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Pickups;

public abstract class Pickup : Entity
{
    private const double _minPositionChangeSeconds = 0.45;
    private const double _maxPositionChangeSeconds = 0.55;
    
    protected readonly int _pickupType;
    private readonly CountdownTimer _positionChangeTimer;
    private bool _isAtStartingPosition;
    
    protected Pickup(Level level, Position position, PickupConfig config)
        : base(level, position, config.BoundingBox)
    {
        _pickupType = config.PickupType;
        double positionChangeSeconds 
            = _minPositionChangeSeconds + (RandomSingleton.Instance.NextDouble() *
            (_maxPositionChangeSeconds - _minPositionChangeSeconds));
        _positionChangeTimer = new CountdownTimer(positionChangeSeconds);
        _isAtStartingPosition = true;
    }

    public override void Update(double elapsedTime)
    {
        _positionChangeTimer.Update(elapsedTime);
        if(_positionChangeTimer.IsFinished)
        {
            _positionChangeTimer.Reset();
            _position.Z += 1 * (_isAtStartingPosition ? 1 : -1);
            _isAtStartingPosition = !_isAtStartingPosition;
        }
    }

    public virtual void PickUp(Survivor survivor)
    {
        Removed = true;
    }
}