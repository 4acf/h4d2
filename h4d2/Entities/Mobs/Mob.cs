using H4D2.Levels;

namespace H4D2.Entities.Mobs;

public abstract class Mob : Entity
{
    protected int _health;
    public int Health => _health;

    protected double _speed;
    protected double _directionRadians;
    protected double _angularVelocity;
    
    protected Mob(Level level, int health, double speed, int xPosition, int yPosition) : base(level, xPosition, yPosition)
    {
        _health = health;
        _speed = speed;
        _directionRadians = 0;
        _angularVelocity = 0;
    }
}