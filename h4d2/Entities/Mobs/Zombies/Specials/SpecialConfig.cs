using H4D2.Infrastructure;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using ZCfg = ZombieConfig;

public static class SpecialConfig
{
    public const int Hunter = 0;
    public const int HunterHealth = 250;
    public const int HunterRunSpeed = 250;
    public static readonly BoundingBox HunterBoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 7, 2, 2, 9, Art.SpriteSize);
    public const int HunterColor = 0x785953;
    
    public const int Boomer = 1;
    public const int BoomerHealth = 50;
    public const int BoomerRunSpeed = 175;
    public static readonly BoundingBox BoomerBoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 5, 6, 6, 9, Art.SpriteSize);
    public const int BoomerColor = 0x847b71;
    
    public const int Smoker = 2;
    public const int SmokerHealth = 250;
    public const int SmokerRunSpeed = 210;
    public static readonly BoundingBox SmokerBoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 7, 2, 2, 12, Art.SpriteSize);
    public const int SmokerColor = 0x7f7165;
    
    public const int Charger = 3;
    public const int ChargerHealth = 600;
    public const int ChargerRunSpeed = 250;
    public static readonly BoundingBox ChargerBoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 4, 7, 7, 12, Art.SpriteSize);
    public const int ChargerColor = 0x435444;
    
    public const int Jockey = 4;
    public const int JockeyHealth = 325;
    public const int JockeyRunSpeed = 250;
    public static readonly BoundingBox JockeyBoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 6, 4, 4, 6, Art.SpriteSize);
    public const int JockeyColor = 0xbc9e9e;
    
    public const int Spitter = 5;
    public const int SpitterHealth = 100;
    public const int SpitterRunSpeed = 210;
    public static readonly BoundingBox SpitterBoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 7, 2, 2, 12, Art.SpriteSize);
    public const int SpitterColor = 0xbc9e9e;
    
    public const int Tank = 6;
    public const int TankHealth = 6000;
    public const int TankRunSpeed = 210;
    public static readonly BoundingBox TankBoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 3, 10, 8, 12, Art.SpriteSize);
    public const int TankColor = 0xd6896b;
    
    public const int Witch = 7;
    public const int WitchHealth = 1000;
    public const int WitchRunSpeed = 300;
    public static readonly BoundingBox WitchBoundingBox = new(ZCfg.CollisionMask, ZCfg.CollidesWith, 7, 2, 2, 10, Art.SpriteSize);
    public const int WitchColor = 0xbcb8b8;
}