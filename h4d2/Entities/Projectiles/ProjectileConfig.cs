namespace H4D2.Entities.Projectiles;

public static class ProjectileConfig
{
    public const int CollisionMask = 0b1;
    public const int CollidesWith = 0b10;

    public static readonly BoundingBox BulletBoundingBox =
        new (CollisionMask, CollidesWith, 1, 1, 1, 0);

    public static readonly BoundingBox GrenadeBoundingBox =
        new(CollisionMask, CollidesWith, 2, 2, 2, 0);
}