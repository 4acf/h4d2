using H4D2.Levels;

namespace H4D2.Entities.Mobs;

public abstract class Mob : Entity
{
    protected int _health;

    protected double _speed;
    protected double _directionRadians;
    protected bool _xFlip;
    protected const double _turnSpeed = 5.0;
    protected const double _speedFactor = 15.0 / 220.0;
    
    protected int _walkStep;
    protected int _walkFrame;
    protected int _lowerFrame;
    protected int _upperFrame;
    protected const double _frameDuration = 1.0 / 8.0;
    protected double _timeSinceLastFrameUpdate;

    protected const int _upperBitmapOffset = 9;
    protected const int _attackingBitmapOffset = 18;
    
    protected Mob(Level level, BoundingBox boundingBox, int health, double speed, int xPosition, int yPosition) :
        base(level, boundingBox, xPosition, yPosition, 0)
    {
        _health = health;
        _speed = speed;
        _directionRadians = 0;
        _xFlip = false;
        _walkStep = 0;
        _walkFrame = 0;
        _lowerFrame = 0;
        _upperFrame = _upperBitmapOffset;
        _timeSinceLastFrameUpdate = 0.0;
    }

    public bool IsAlive() => _health > 0;
}