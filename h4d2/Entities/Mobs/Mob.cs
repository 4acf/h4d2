using H4D2.Entities.Mobs.Zombies;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Mobs;

public abstract class Mob : Entity
{
    public bool IsAlive => _health > 0;
    
    protected const double _turnSpeed = 5.0;
    protected const double _speedFactor = 15.0 / 220.0;
    protected const int _upperBitmapOffset = 9;
    protected const int _attackingBitmapOffset = 18;
    protected const double _frameDuration = 1.0 / 8.0;
    
    protected int _health;
    protected double _speed;

    protected double _directionRadians;
    protected bool _xFlip;
    protected int _walkStep;
    protected int _walkFrame;
    protected int _lowerFrame;
    protected int _upperFrame;
    protected double _timeSinceLastFrameUpdate;
    protected readonly int _color;
    
    protected Mob(Level level, BoundingBox boundingBox, int health, double speed, int xPosition, int yPosition, int color) :
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
        _color = color;
    }
    
    public void HitBy(Zombie zombie)
    {
        if (Removed)
            return;
        _health -= zombie.Damage;
        if (!IsAlive)
        {
            _Die();
        }
        (double x, double y, double z) = CenterMass;
        var bloodSplatter = new BloodSplatterDebris(_level, x, y, z);
        _level.AddParticle(bloodSplatter);
    }

    protected virtual void _Die()
    {
        (double x, double y, double z) = CenterMass;
        for (int i = 0; i < 8; i++)
        {
            var deathSplatter = new DeathSplatterDebris(_level, x, y, z + i, _color);
            _level.AddParticle(deathSplatter);
        }
        Removed = true;
    }
}