using H4D2.Infrastructure;

namespace H4D2.Entities.Pickups.ThrowablePickup;

public class ThrowablePickupConfig : PickupConfig
{
    public required int ThrowablePickupType { get; init; }
}

public static class ThrowablePickupConfigs
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

    public static readonly ThrowablePickupConfig Molotov = new()
    {
        PickupType = _pickupType,
        ThrowablePickupType = 0,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _molotovBoundingBoxDimensions)
    };

    public static readonly ThrowablePickupConfig PipeBomb = new()
    {
        PickupType = _pickupType,
        ThrowablePickupType = 1,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _pipeBombBoundingBoxDimensions)
    };

    public static readonly ThrowablePickupConfig BileBomb = new()
    {
        PickupType = _pickupType,
        ThrowablePickupType = 2,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _bileBombBoundingBoxDimensions)
    };
}