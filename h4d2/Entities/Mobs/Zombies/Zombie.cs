using H4D2.Entities.Projectiles;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Mobs.Zombies;

public abstract class Zombie : Mob
{
    protected Entity? _target;
    protected bool _isAttacking;
    public readonly int Damage;
    
    protected Zombie(Level level, BoundingBox boundingBox, int health, double speed, int damage, int xPosition, int yPosition, int color)
        : base(level, boundingBox, health, speed, xPosition, yPosition, color)
    {
        _target = null;
        _isAttacking = false;
        Damage = damage;
    }

    public void HitBy(Projectile projectile)
    {
        if (Removed)
            return;
        _health -= projectile.Damage;
        if (_health <= 0)
        {
            _Die();
        }
        var (x, y, z) = BoundingBox.CenterMass(XPosition, YPosition, ZPosition);
        var bloodSplatter = new BloodSplatterDebris(_level, x, y, z);
        _level.AddParticle(bloodSplatter);
    }
}