using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public abstract class Projectile : Entity
{
    public readonly int Damage;
    protected readonly double _directionRadians;
    
    protected Projectile(Level level, BoundingBox boundingBox, double xPosition, double yPosition, double zPosition, int damage, double directionRadians) 
        : base(level, boundingBox, xPosition, yPosition, zPosition)
    {
        Damage = damage;
        _directionRadians = directionRadians;
    }
}