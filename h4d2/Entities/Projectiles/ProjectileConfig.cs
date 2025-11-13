using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Projectiles;

public static class ProjectileConfig
{
    
    private static readonly BoundingBoxDimensions _bulletDimensions
        = new(1, 1, 1, 0);

    private static readonly BoundingBoxDimensions _grenadeDimensions
        = new(2, 2, 2, 0);

    private static readonly BoundingBoxDimensions _spitDimensions
        = new(2, 2, 2, 0);
    
    public static readonly BoundingBox BulletBoundingBox 
        = new (CollisionGroup.Projectile, _bulletDimensions);

    public static readonly BoundingBox GrenadeBoundingBox 
        = new(CollisionGroup.Projectile, _grenadeDimensions);

    public static readonly BoundingBox SpitBoundingBox
        = new(CollisionGroup.ZombieProjectile, _spitDimensions);
}