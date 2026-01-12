using H4D2.Entities.Hazards;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Emitters;

namespace H4D2.Entities.Mobs.Zombies;

public abstract class Zombie : Mob
{
    private const double _fireDamageMultipler = 75.0;
    
    public readonly int Damage;
    public double DirectionRadians => _directionRadians;
    protected Entity? _target;
    protected bool _isAttacking;
    
    protected Zombie(Level level, Position position, ZombieConfig config)
        : base(level, position, config)
    {
        _target = null;
        _isAttacking = false;
        Damage = config.Damage;
    }

    protected Zombie(Level level, Position position, ZombieConfig config, int speed)
        : base(level, position, config, speed)
    {
        _target = null;
        _isAttacking = false;
        Damage = config.Damage;
    }
    
    public virtual void HitBy(Projectile projectile)
    {
        if (Removed || projectile.Removed)
            return;
        _health -= projectile.Damage;
        if (!IsAlive)
        {
            _Die();
        }
        var bloodSplatter = new BloodSplatter(_level, CenterMass.MutableCopy());
        _level.AddParticle(bloodSplatter);
    }
    
    protected override void _Collide(Entity? entity)
    {
        switch (entity)
        {
            case null:
                base._Collide(entity);
                return;
            case Fire fire:
                _TakeHazardDamage((int)(fire.Damage * _fireDamageMultipler));
                break;
            case SpitPuddle:
                break;
            default:
                base._Collide(entity);
                break;
        }
    }
}