using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Pickups.Consumables;

public class ConsumableConfig : PickupConfig
{
    public required int ConsumableType { get; init; }
}

public static class ConsumableConfigs
{
    private const int _pickupType = 0;

    private static readonly BoundingBoxDimensions _firstAidKitBoundingBoxDimensions
        = new(5, 2, 8, H4D2Art.PickupSize);
    
    private static readonly BoundingBoxDimensions _pillsBoundingBoxDimensions
        = new(4, 2, 6, H4D2Art.PickupSize, 2);

    private static readonly BoundingBoxDimensions _adrenalineBoundingBoxDimensions
        = new(3, 2, 3, H4D2Art.PickupSize, 2);
    
    public static readonly ConsumableConfig FirstAidKit = new()
    {
        PickupType = _pickupType,
        ConsumableType = 0,
        BoundingBox = new BoundingBox(CollisionGroup.Pickup, _firstAidKitBoundingBoxDimensions)
    };
    
    public static readonly ConsumableConfig Pills = new()
    {
        PickupType = _pickupType,
        ConsumableType = 1,
        BoundingBox = new BoundingBox(CollisionGroup.Pickup, _pillsBoundingBoxDimensions)
    };

    public static readonly ConsumableConfig Adrenaline = new()
    {
        PickupType = _pickupType,
        ConsumableType = 2,
        BoundingBox = new BoundingBox(CollisionGroup.Pickup, _adrenalineBoundingBoxDimensions)
    };
}