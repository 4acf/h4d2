using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Mobs.Zombies;

public abstract class Zombie : Mob
{
    protected Entity? _target;
    protected bool _isAttacking;
    public readonly int Damage;
    
    protected Zombie(Level level, Position position, ZombieConfig config)
        : base(level, config.BoundingBox, position, config.Health, config.RunSpeed, config.GibColor)
    {
        _target = null;
        _isAttacking = false;
        Damage = config.Damage;
    }

    public void HitBy(Projectile projectile)
    {
        if (Removed || projectile.Removed)
            return;
        _health -= projectile.Damage;
        if (!IsAlive)
        {
            _Die();
        }
        var bloodSplatter = new BloodSplatterDebris(_level, CenterMass.MutableCopy());
        _level.AddParticle(bloodSplatter);
    }
}