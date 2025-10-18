using H4D2.Levels;

namespace H4D2.Entities.Mobs;

public abstract class Mob : Entity
{
    protected int _health;
    public int Health => _health;

    protected double _speed;
    protected double _directionRadians;
    protected bool _xFlip;
    protected const double _turnSpeed = 5.0;
    
    protected int _walkStep;
    protected int _walkFrame;
    protected const double _frameDuration = 1.0 / 8.0;
    protected double _timeSinceLastFrameUpdate;
    
    protected Mob(Level level, BoundingBox boundingBox, int health, double speed, int xPosition, int yPosition) :
        base(level, boundingBox, xPosition, yPosition)
    {
        _health = health;
        _speed = speed;
        _directionRadians = 0;
        _xFlip = false;
        _walkStep = 0;
        _walkFrame = 0;
        _timeSinceLastFrameUpdate = 0.0;
    }
}