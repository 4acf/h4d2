using H4D2.Entities.Projectiles;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Mobs.Zombies;

public abstract class Zombie : Mob
{
    protected Entity? _target;
    
    protected Zombie(Level level, BoundingBox boundingBox, int health, double speed, int xPosition, int yPosition)
        : base(level, boundingBox, health, speed, xPosition, yPosition)
    {
        _target = null;
    }

    public void HitBy(Projectile projectile)
    {
        _health -= projectile.Damage;
        if (_health <= 0)
        {
            Removed = true;
        }
        var (x, y, z) = BoundingBox.CenterMass(XPosition, YPosition, ZPosition);
        var bloodSplatter = new BloodSplatterDebris(_level, x, y, z);
        _level.AddParticle(bloodSplatter);
    }
}