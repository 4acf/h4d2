using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public abstract class Projectile : Entity
{
    protected readonly double _directionRadians;
    
    protected Projectile(Level level, BoundingBox boundingBox, double directionRadians, double xPosition, double yPosition) 
        : base(level, boundingBox, xPosition, yPosition)
    {
        _directionRadians = directionRadians;
    }
}