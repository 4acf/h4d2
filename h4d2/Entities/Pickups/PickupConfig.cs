using H4D2.Infrastructure;

namespace H4D2.Entities.Pickups;

public class PickupConfig
{
    public required int PickupType { get; init; }
    public required BoundingBox BoundingBox { get; init; }
}

public static class PickupCollision
{
    public const int CollisionMask = 0b1000;
    public const int CollidesWith = 0b0;
}