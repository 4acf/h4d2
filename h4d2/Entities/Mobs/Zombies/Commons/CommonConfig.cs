using H4D2.Infrastructure;

namespace H4D2.Entities.Mobs.Zombies.Commons;
using ZCfg = ZombieConfig;

public static class CommonConfig
{
    public static readonly BoundingBox BoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 7, 2, 2, 10, Art.SpriteSize);
    public const int Health = 50;
    public const int MinSpeed = 230;
    public const int MaxSpeed = 280;
    public const int NumSprites = 9;
}