using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;

public class UncommonConfig : ZombieConfig
{
    public required int Type { get; init; }
}

public static class UncommonConfigs
{
    private static readonly BoundingBoxDimensions _boundingBoxDimensions
        = new(2, 2, 10, Art.SpriteSize, 7);
    private static readonly BoundingBox _boundingBox 
        = new(CollisionGroup.Zombie, _boundingBoxDimensions);
    private const int _damage = 2;

    public static readonly UncommonConfig Hazmat = new()
    {
        Type = 0,
        Health = 200,
        RunSpeed = 250,
        Damage = _damage,
        GibColor = 0x847b71,
        BoundingBox = _boundingBox
    };

    public static readonly UncommonConfig Clown = new()
    {
        Type = 1,
        Health = 200,
        RunSpeed = 250,
        Damage = _damage,
        GibColor = 0x847b71,
        BoundingBox = _boundingBox
    };

    public static readonly UncommonConfig Mudman = new()
    {
        Type = 2,
        Health = 200,
        RunSpeed = 300,
        Damage = _damage,
        GibColor = 0x856e55,
        BoundingBox = _boundingBox
    };

    public static readonly UncommonConfig Worker = new()
    {
        Type = 3,
        Health = 200,
        RunSpeed = 250,
        Damage = _damage,
        GibColor = 0x847b71,
        BoundingBox = _boundingBox
    };

    public static readonly UncommonConfig Riot = new()
    {
        Type = 4,
        Health = 200,
        RunSpeed = 230,
        Damage = _damage,
        GibColor = 0xa7a39e,
        BoundingBox = _boundingBox
    };
}