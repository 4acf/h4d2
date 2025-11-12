using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Hazards;

public class HazardConfig
{
    public required int Damage { get; init; }
    public required double Duration { get; init; }
    public required BoundingBox BoundingBox { get; init; }
}

public static class HazardConfigs
{
    private static readonly BoundingBoxDimensions _fireDimensions 
        = new(4, 4, 3, H4D2Art.ParticleSize, 2);
    
    public static readonly HazardConfig Fire = new()
    {
        Damage = 10,
        Duration = 15.0,
        BoundingBox = new BoundingBox(CollisionGroup.Hazard, _fireDimensions)
    };
}