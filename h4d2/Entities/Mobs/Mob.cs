using H4D2.Entities.Mobs.Zombies;
using H4D2.Infrastructure;
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
    protected readonly int _gibColor;
    
    protected Mob(Level level, Position position, MobConfig config) :
        base(level, position, config.BoundingBox)
    {
        _health = config.Health;
        _speed = config.RunSpeed;
        _directionRadians = 0;
        _xFlip = false;
        _walkStep = 0;
        _walkFrame = 0;
        _lowerFrame = 0;
        _upperFrame = _upperBitmapOffset;
        _timeSinceLastFrameUpdate = 0.0;
        _gibColor = config.GibColor;
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
        var bloodSplatter = new BloodSplatterDebris(_level, CenterMass.MutableCopy());
        _level.AddParticle(bloodSplatter);
    }

    protected virtual void _Die()
    {
        for (int i = 0; i < 8; i++)
        {
            Position position = CenterMass.MutableCopy();
            position.Z += i;
            var deathSplatter = new DeathSplatterDebris(_level, position, _gibColor);
            _level.AddParticle(deathSplatter);
        }
        Removed = true;
    }
}