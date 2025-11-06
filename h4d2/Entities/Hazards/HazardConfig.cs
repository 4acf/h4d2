using H4D2.Infrastructure;

namespace H4D2.Entities.Hazards;

public class HazardConfig
{
    public required int Damage { get; init; }
    public required double TimeToLiveSeconds { get; init; }
    public required BoundingBox BoundingBox { get; init; }
}

public static class HazardConfigs
{
    private const int _collisionMask = 0b10000;
    private const int _collidesWith = 0b0;
    
    private static readonly BoundingBoxDimensions _fireDimensions 
        = new(4, 4, 3, Art.ParticleSize, 2);
    
    public static readonly HazardConfig Fire = new()
    {
        Damage = 10,
        TimeToLiveSeconds = 15.0,
        BoundingBox = new BoundingBox(_collisionMask, _collidesWith, _fireDimensions)
    };
}