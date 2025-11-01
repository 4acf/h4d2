namespace H4D2.Entities.Projectiles;

public static class ProjectileConfig
{
    private const int _collisionMask = 0b1;
    private const int _collidesWith = 0b10;

    private static readonly BoundingBoxDimensions _bulletDimensions
        = new(1, 1, 1, 0);

    private static readonly BoundingBoxDimensions _grenadeDimensions
        = new(2, 2, 2, 0);
    
    public static readonly BoundingBox BulletBoundingBox =
        new (_collisionMask, _collidesWith, _bulletDimensions);

    public static readonly BoundingBox GrenadeBoundingBox =
        new(_collisionMask, _collidesWith, _grenadeDimensions);
}