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
    
    protected Zombie(Level level, BoundingBox boundingBox, Position position, int health, double speed, int damage, int color)
        : base(level, boundingBox, position, health, speed, color)
    {
        _target = null;
        _isAttacking = false;
        Damage = damage;
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