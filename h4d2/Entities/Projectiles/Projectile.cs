using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public abstract class Projectile : Entity
{
    protected readonly double _directionRadians;
    public readonly int Damage;
    
    protected Projectile(Level level, BoundingBox boundingBox, double directionRadians, double xPosition, double yPosition, int damage) 
        : base(level, boundingBox, xPosition, yPosition)
    {
        _directionRadians = directionRadians;
        Damage = damage;
    }
}