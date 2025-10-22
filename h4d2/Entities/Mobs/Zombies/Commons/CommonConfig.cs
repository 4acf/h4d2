namespace H4D2.Entities.Mobs.Zombies.Commons;
using ZCfg = ZombieConfig;

public static class CommonConfig
{
    public static readonly BoundingBox BoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 4, 6, 8, 10);
    public const int Health = 50;
    public const int MinSpeed = 230;
    public const int MaxSpeed = 280;
    public const int NumSprites = 9;
}