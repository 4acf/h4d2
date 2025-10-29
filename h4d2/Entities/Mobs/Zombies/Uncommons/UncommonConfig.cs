using H4D2.Infrastructure;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using ZCfg = ZombieConfig;

public static class UncommonConfig
{
    public static readonly BoundingBox BoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 7, 2, 2, 10, Art.SpriteSize);
    public const int Damage = 2;
    
    public const int Hazmat = 0;
    public const int HazmatHealth = 200;
    public const int HazmatSpeed = 250;
    public const int HazmatColor = 0x847b71;
    
    public const int Clown = 1;
    public const int ClownHealth = 200;
    public const int ClownSpeed = 250;
    public const int ClownColor = 0x847b71;
    
    public const int Mudman = 2;
    public const int MudmanHealth = 200;
    public const int MudmanSpeed = 300;
    public const int MudmanColor = 0x856e55;
    
    public const int Worker = 3;
    public const int WorkerHealth = 200;
    public const int WorkerSpeed = 250;
    public const int WorkerColor = 0x847b71;
    
    public const int Riot = 4;
    public const int RiotHealth = 200;
    public const int RiotSpeed = 230;
    public const int RiotColor = 0xa7a39e;
}