using H4D2.Infrastructure;

namespace H4D2.Entities.Mobs;

public abstract class Mob : Entity
{
    protected int _health;
    public int Health => _health;

    protected double _speed;
    public double Speed => _speed;
    
    protected Mob(int health, double speed, int xPosition, int yPosition) : base(xPosition, yPosition)
    {
        _health = health;
        _speed = speed;
    }
}