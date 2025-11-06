using H4D2.Infrastructure;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;
using PCol = ProjectileCollision;

public class ThrowableProjectileConfig
{
    public required int Type { get; init; }
    public required int Damage { get; init; }
    public required BoundingBox BoundingBox { get; init; }
}

public static class ThrowableProjectileConfigs
{
    private static readonly BoundingBoxDimensions _molotovDimensions
        = new(4, 4, 4, Art.ProjectileSize, 2);
    
    private static readonly BoundingBoxDimensions _pipeBombDimensions
        = new(2, 2, 2, Art.ProjectileSize, 3);

    private static readonly BoundingBoxDimensions _bileBombDimensions
        = new(3, 3, 3, Art.ProjectileSize, 2);
    
    private static readonly BoundingBox _molotovBoundingBox 
        = new(PCol.CollisionMask, PCol.CollidesWith, _molotovDimensions);
    
    private static readonly BoundingBox _pipeBombBoundingBox 
        = new(PCol.CollisionMask, PCol.CollidesWith, _pipeBombDimensions);

    private static readonly BoundingBox _bileBombBoundingBox 
        = new(PCol.CollisionMask, PCol.CollidesWith, _bileBombDimensions);
    
    public static readonly ThrowableProjectileConfig Molotov = new()
    {
        Type = 0,
        Damage = 0,
        BoundingBox = _molotovBoundingBox
    };

    public static readonly ThrowableProjectileConfig PipeBomb = new()
    {
        Type = 1,
        Damage = 250,
        BoundingBox = _pipeBombBoundingBox
    };

    public static readonly ThrowableProjectileConfig BileBomb = new()
    {
        Type = 2,
        Damage = 0,
        BoundingBox = _bileBombBoundingBox
    };
}