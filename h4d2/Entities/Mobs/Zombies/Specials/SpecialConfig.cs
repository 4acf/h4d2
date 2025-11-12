using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class SpecialConfig : ZombieConfig
{
    public required int Type { get; init; }
} 

public static class SpecialConfigs
{
    private static readonly BoundingBoxDimensions _hunterDimensions
        = new(2, 2, 9, H4D2Art.SpriteSize, 7);
    private static readonly BoundingBoxDimensions _boomerDimensions
        = new(6, 6, 9, H4D2Art.SpriteSize, 5);
    private static readonly BoundingBoxDimensions _smokerDimensions
        = new(2, 2, 12, H4D2Art.SpriteSize, 7);
    private static readonly BoundingBoxDimensions _chargerDimensions
        = new(7, 7, 12, H4D2Art.SpriteSize, 4);
    private static readonly BoundingBoxDimensions _jockeyDimensions
        = new(4, 4, 6, H4D2Art.SpriteSize, 6);
    private static readonly BoundingBoxDimensions _spitterDimensions
        = new(2, 2, 12, H4D2Art.SpriteSize, 7);
    private static readonly BoundingBoxDimensions _tankDimensions
        = new(10, 8, 12, H4D2Art.SpriteSize, 3);
    private static readonly BoundingBoxDimensions _witchDimensions
        = new(2, 2, 10, H4D2Art.SpriteSize, 7);
    
    public static readonly SpecialConfig Hunter = new()
    {
        Type = 0,
        Health = 250,
        RunSpeed = 250,
        Damage = 5,
        GibColor = 0x785953,
        BoundingBox = new BoundingBox(CollisionGroup.Zombie, _hunterDimensions)
    };

    public static readonly SpecialConfig Boomer = new()
    {
        Type = 1,
        Health = 50,
        RunSpeed = 175,
        Damage = 0,
        GibColor = 0x847b71,
        BoundingBox = new BoundingBox(CollisionGroup.Zombie, _boomerDimensions)
    };

    public static readonly SpecialConfig Smoker = new()
    {
        Type = 2,
        Health = 250,
        RunSpeed = 210,
        Damage = 5,
        GibColor = 0x7f7165,
        BoundingBox = new BoundingBox(CollisionGroup.Zombie, _smokerDimensions)
    };

    public static readonly SpecialConfig Charger = new()
    {
        Type = 3,
        Health = 600,
        RunSpeed = 250,
        Damage = 15,
        GibColor = 0x435444,
        BoundingBox = new BoundingBox(CollisionGroup.Zombie, _chargerDimensions)
    };

    public static readonly SpecialConfig Jockey = new()
    {
        Type = 4,
        Health = 325,
        RunSpeed = 250,
        Damage = 4,
        GibColor = 0xbc9e9e,
        BoundingBox = new BoundingBox(CollisionGroup.Zombie, _jockeyDimensions)
    };

    public static readonly SpecialConfig Spitter = new()
    {
        Type = 5,
        Health = 100,
        RunSpeed = 210,
        Damage = 7,
        GibColor = 0xbc9e9e,
        BoundingBox = new BoundingBox(CollisionGroup.Zombie, _spitterDimensions)
    };

    public static readonly SpecialConfig Tank = new()
    {
        Type = 6,
        Health = 6000,
        RunSpeed = 210,
        Damage = 24,
        GibColor = 0xd6896b,
        BoundingBox = new BoundingBox(CollisionGroup.Zombie, _tankDimensions)
    };

    public static readonly SpecialConfig Witch = new()
    {
        Type = 7,
        Health = 1000,
        RunSpeed = 300,
        Damage = 100,
        GibColor = 0xbcb8b8,
        BoundingBox = new BoundingBox(CollisionGroup.Zombie, _witchDimensions)
    };
}