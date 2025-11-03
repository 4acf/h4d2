using H4D2.Infrastructure;

namespace H4D2.Entities.Pickups.Consumables;

public class ConsumableConfig : PickupConfig
{
    public required int ConsumableType { get; init; }
}

public static class ConsumableConfigs
{
    private const int _pickupType = 0;
    private const int _collisionMask = PickupCollision.CollisionMask;
    private const int _collidesWith = PickupCollision.CollidesWith;

    private static readonly BoundingBoxDimensions _firstAidKitBoundingBoxDimensions
        = new(5, 2, 8, Art.PickupSize);
    
    private static readonly BoundingBoxDimensions _pillsBoundingBoxDimensions
        = new(4, 2, 6, Art.PickupSize, 2);

    private static readonly BoundingBoxDimensions _adrenalineBoundingBoxDimensions
    = new(3, 2, 3, Art.PickupSize, 2);
    
    public static readonly ConsumableConfig FirstAidKit = new()
    {
        PickupType = _pickupType,
        ConsumableType = 0,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _firstAidKitBoundingBoxDimensions)
    };
    
    public static readonly ConsumableConfig Pills = new()
    {
        PickupType = _pickupType,
        ConsumableType = 1,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _pillsBoundingBoxDimensions)
    };

    public static readonly ConsumableConfig Adrenaline = new()
    {
        PickupType = _pickupType,
        ConsumableType = 2,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _adrenalineBoundingBoxDimensions)
    };
}