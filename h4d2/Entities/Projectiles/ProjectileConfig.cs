namespace H4D2.Entities.Projectiles;

public static class ProjectileConfig
{
    
    private static readonly BoundingBoxDimensions _bulletDimensions
        = new(1, 1, 1, 0);

    private static readonly BoundingBoxDimensions _grenadeDimensions
        = new(2, 2, 2, 0);
    
    public static readonly BoundingBox BulletBoundingBox 
        = new (ProjectileCollision.CollisionMask, ProjectileCollision.CollidesWith, _bulletDimensions);

    public static readonly BoundingBox GrenadeBoundingBox 
        = new(ProjectileCollision.CollisionMask, ProjectileCollision.CollidesWith, _grenadeDimensions);
}

public static class ProjectileCollision
{
    public const int CollisionMask = 0b1;
    public const int CollidesWith = 0b10;
}