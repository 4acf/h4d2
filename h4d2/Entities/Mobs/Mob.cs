using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles;
using H4D2.Particles.DebrisParticles.Emitters;

namespace H4D2.Entities.Mobs;

public abstract class Mob : Entity
{
    public bool IsAlive => _health > 0;
    
    protected const double _turnSpeed = 5.0;
    protected const double _speedFactor = 15.0 / 220.0;
    protected const int _upperBitmapOffset = 9;
    protected const int _attackingBitmapOffset = 18;
    protected const double _frameDuration = 1.0 / 8.0;
    protected const double _hazardDamageCooldownSeconds = 0.5;
    private const double _punchedScale = 2.5;
    private const double _punchedZVelocity = 1.0;
    
    protected int _health;
    protected double _speed;

    protected double _directionRadians;
    protected bool _xFlip;
    protected int _walkStep;
    protected int _lowerFrame;
    protected int _upperFrame;
    protected readonly int _gibColor;
    protected readonly CountdownTimer _frameUpdateTimer;
    protected readonly CountdownTimer _hazardDamageTimer;
    
    protected Mob(Level level, Position position, MobConfig config) 
        : base(level, position, config.BoundingBox)
    {
        _health = config.Health;
        _speed = config.RunSpeed;
        _directionRadians = 0.0;
        _xFlip = false;
        _walkStep = 0;
        _lowerFrame = 0;
        _upperFrame = _upperBitmapOffset;
        _gibColor = config.GibColor;
        _frameUpdateTimer = new CountdownTimer(_frameDuration);
        _hazardDamageTimer = new CountdownTimer(_hazardDamageCooldownSeconds);
    }

    protected Mob(Level level, Position position, MobConfig config, double speed)
        : base(level, position, config.BoundingBox)
    {
        _health = config.Health;
        _speed = speed;
        _directionRadians = 0;
        _xFlip = false;
        _walkStep = 0;
        _lowerFrame = 0;
        _upperFrame = _upperBitmapOffset;
        _gibColor = config.GibColor;
        _frameUpdateTimer = new CountdownTimer(_frameDuration);
        _hazardDamageTimer = new CountdownTimer(_hazardDamageCooldownSeconds);
    }
    
    public virtual void HitBy(Zombie zombie)
    {
        if (Removed)
            return;
        _health -= zombie.Damage;
        if (!IsAlive)
        {
            _Die();
        }
        var bloodSplatter = new BloodSplatter(_level, CenterMass.MutableCopy());
        _level.AddParticle(bloodSplatter);

        if (zombie is not Tank tank)
            return;

        _velocity.X = Math.Cos(tank.AimDirectionRadians) * _punchedScale;
        _velocity.Y = Math.Sin(tank.AimDirectionRadians) * _punchedScale;
        _velocity.Z = _punchedZVelocity;
    }
    
    protected virtual void _TakeHazardDamage(int damage)
    {
        if (Removed)
            return;
        if (!_hazardDamageTimer.IsFinished)
            return;
        _hazardDamageTimer.Reset();
        _health -= damage;
        if (!IsAlive)
        {
            _Die();
        }
        var bloodSplatter = new BloodSplatter(_level, CenterMass.MutableCopy());
        _level.AddParticle(bloodSplatter);
    }
    
    protected virtual void _Die()
    {
        for (int i = 0; i < 8; i++)
        {
            Position position = CenterMass.MutableCopy();
            position.Z += i;
            var deathSplatter = new GibDebris(_level, position, _gibColor);
            _level.AddParticle(deathSplatter);
        }
        Removed = true;
    }
}