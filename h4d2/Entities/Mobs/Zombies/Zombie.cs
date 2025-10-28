using H4D2.Entities.Projectiles;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Mobs.Zombies;

public abstract class Zombie : Mob
{
    protected Entity? _target;
    protected readonly int _color;
    protected bool _isAttacking;
    
    protected Zombie(Level level, BoundingBox boundingBox, int health, double speed, int xPosition, int yPosition, int color)
        : base(level, boundingBox, health, speed, xPosition, yPosition)
    {
        _target = null;
        _color = color;
        _isAttacking = false;
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

    protected void _Die()
    {
        var (x, y, z) = BoundingBox.CenterMass(XPosition, YPosition, ZPosition);
        for (int i = 0; i < 8; i++)
        {
            var deathSplatter = new DeathSplatterDebris(_level, x, y, z + i, _color);
            _level.AddParticle(deathSplatter);
        }
        Removed = true;
    }
}