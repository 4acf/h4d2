using H4D2.Infrastructure;

namespace H4D2.Entities.Mobs.Zombies.Commons;
using ZCol = ZombieCollision;

public class CommonConfig : ZombieConfig;

public static class CommonConfigs
{
    public static readonly CommonConfig Common = new()
    {
        Health = 50,
        RunSpeed = 250,
        Damage = 2,
        GibColor = 0x847b71,
        BoundingBox = new BoundingBox(ZCol.CollisionMask, ZCol.CollidesWith, 7, 2, 2, 10, Art.SpriteSize)
    };
}