using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public abstract class Projectile : Entity
{
    public readonly int Damage;
    protected double _directionRadians;
    
    protected Projectile(Level level, Position position, BoundingBox boundingBox, int damage, double directionRadians) 
        : base(level, position, boundingBox)
    {
        Damage = damage;
        _directionRadians = directionRadians;
    }
}