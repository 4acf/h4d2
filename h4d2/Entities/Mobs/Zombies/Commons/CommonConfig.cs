using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Mobs.Zombies.Commons;

public class CommonConfig : ZombieConfig;

public static class CommonConfigs
{
    private static readonly BoundingBoxDimensions _boundingBoxDimensions
        = new(2, 2, 10, H4D2Art.SpriteSize, 7);

    private static readonly BoundingBox _boundingBox
        = new(CollisionGroup.Zombie, _boundingBoxDimensions);
    
    public static readonly CommonConfig Common = new()
    {
        Health = 50,
        RunSpeed = 250,
        Damage = 2,
        GibColor = 0x847b71,
        BoundingBox = _boundingBox
    };
}