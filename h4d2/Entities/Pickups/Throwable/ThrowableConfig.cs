using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Pickups.Throwable;

public class ThrowableConfig : PickupConfig
{
    public required int ThrowableType { get; init; }
}

public static class ThrowableConfigs
{
    private const int _pickupType = 1;

    private static readonly BoundingBoxDimensions _molotovBoundingBoxDimensions
        = new(4, 2, 6, H4D2Art.PickupSize, 2);
    
    private static readonly BoundingBoxDimensions _pipeBombBoundingBoxDimensions
        = new(2, 2, 7, H4D2Art.PickupSize, 2);

    private static readonly BoundingBoxDimensions _bileBombBoundingBoxDimensions
        = new(3, 3, 7, H4D2Art.PickupSize, 2);

    public static readonly ThrowableConfig Molotov = new()
    {
        PickupType = _pickupType,
        ThrowableType = 0,
        BoundingBox = new BoundingBox(CollisionGroup.Pickup, _molotovBoundingBoxDimensions)
    };

    public static readonly ThrowableConfig PipeBomb = new()
    {
        PickupType = _pickupType,
        ThrowableType = 1,
        BoundingBox = new BoundingBox(CollisionGroup.Pickup, _pipeBombBoundingBoxDimensions)
    };

    public static readonly ThrowableConfig BileBomb = new()
    {
        PickupType = _pickupType,
        ThrowableType = 2,
        BoundingBox = new BoundingBox(CollisionGroup.Pickup, _bileBombBoundingBoxDimensions)
    };
}