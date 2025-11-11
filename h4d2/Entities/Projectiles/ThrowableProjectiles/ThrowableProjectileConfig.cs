using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

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
        = new(CollisionGroup.Projectile, _molotovDimensions);
    
    private static readonly BoundingBox _pipeBombBoundingBox 
        = new(CollisionGroup.Projectile, _pipeBombDimensions);

    private static readonly BoundingBox _bileBombBoundingBox 
        = new(CollisionGroup.Projectile, _bileBombDimensions);
    
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