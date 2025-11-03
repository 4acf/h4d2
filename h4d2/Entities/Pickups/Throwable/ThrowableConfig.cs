using H4D2.Infrastructure;

namespace H4D2.Entities.Pickups.Throwable;

public class ThrowableConfig : PickupConfig
{
    public required int ThrowableType { get; init; }
}

public static class ThrowableConfigs
{
    private const int _pickupType = 1;
    private const int _collisionMask = PickupCollision.CollisionMask;
    private const int _collidesWith = PickupCollision.CollidesWith;

    private static readonly BoundingBoxDimensions _molotovBoundingBoxDimensions
        = new(4, 2, 6, Art.PickupSize, 2);
    
    private static readonly BoundingBoxDimensions _pipeBombBoundingBoxDimensions
        = new(2, 2, 7, Art.PickupSize, 2);

    private static readonly BoundingBoxDimensions _bileBombBoundingBoxDimensions
        = new(3, 3, 7, Art.PickupSize, 2);

    public static readonly ThrowableConfig Molotov = new()
    {
        PickupType = _pickupType,
        ThrowableType = 0,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _molotovBoundingBoxDimensions)
    };

    public static readonly ThrowableConfig PipeBomb = new()
    {
        PickupType = _pickupType,
        ThrowableType = 1,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _pipeBombBoundingBoxDimensions)
    };

    public static readonly ThrowableConfig BileBomb = new()
    {
        PickupType = _pickupType,
        ThrowableType = 2,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _bileBombBoundingBoxDimensions)
    };
}