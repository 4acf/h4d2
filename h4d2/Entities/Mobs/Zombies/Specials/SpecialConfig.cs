using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class SpecialConfig : ZombieConfig
{
    public required int Type { get; init; }
}

public static class SpecialIndices
{
    public const int Hunter = 0;
    public const int Boomer = 1;
    public const int Smoker = 2;
    public const int Charger = 3;
    public const int Jockey = 4;
    public const int Spitter = 5;
    public const int Tank = 6;
    public const int Witch = 7;
}

public static class SpecialBoundingBoxes
{
    private static readonly BoundingBoxDimensions _hunterDimensions
        = new(2, 2, 9, H4D2Art.SpriteSize, 7);
    private static readonly BoundingBoxDimensions _boomerDimensions
        = new(6, 6, 9, H4D2Art.SpriteSize, 5);
    private static readonly BoundingBoxDimensions _smokerDimensions
        = new(2, 2, 12, H4D2Art.SpriteSize, 7);
    private static readonly BoundingBoxDimensions _chargerDimensions
        = new(2, 2, 12, H4D2Art.SpriteSize, 7);
    private static readonly BoundingBoxDimensions _jockeyDimensions
        = new(4, 2, 6, H4D2Art.SpriteSize, 6);
    private static readonly BoundingBoxDimensions _spitterDimensions
        = new(2, 2, 12, H4D2Art.SpriteSize, 7);
    private static readonly BoundingBoxDimensions _tankDimensions
        = new(10, 8, 12, H4D2Art.SpriteSize, 3);
    private static readonly BoundingBoxDimensions _witchDimensions
        = new(2, 2, 10, H4D2Art.SpriteSize, 7);
    public static readonly BoundingBox Hunter
        = new(CollisionGroup.Zombie, _hunterDimensions);
    public static readonly BoundingBox Boomer
        = new(CollisionGroup.Zombie, _boomerDimensions);
    public static readonly BoundingBox Smoker
        = new(CollisionGroup.Zombie, _smokerDimensions);
    public static readonly BoundingBox Charger
        = new(CollisionGroup.Zombie, _chargerDimensions);
    public static readonly BoundingBox Jockey
        = new(CollisionGroup.Zombie, _jockeyDimensions);
    public static readonly BoundingBox Spitter
        = new(CollisionGroup.Zombie, _spitterDimensions);
    public static readonly BoundingBox Tank
        = new(CollisionGroup.Zombie, _tankDimensions);
    public static readonly BoundingBox Witch
        = new(CollisionGroup.Zombie, _witchDimensions);
}

public static class SpecialConfigs
{
    public static readonly SpecialConfig Hunter = new()
    {
        Type = SpecialIndices.Hunter,
        Health = 250,
        RunSpeed = 250,
        Damage = 5,
        GibColor = 0x785953,
        BoundingBox = SpecialBoundingBoxes.Hunter
    };

    public static readonly SpecialConfig Boomer = new()
    {
        Type = SpecialIndices.Boomer,
        Health = 50,
        RunSpeed = 175,
        Damage = 0,
        GibColor = 0x847b71,
        BoundingBox = SpecialBoundingBoxes.Boomer
    };

    public static readonly SpecialConfig Smoker = new()
    {
        Type = SpecialIndices.Smoker,
        Health = 250,
        RunSpeed = 210,
        Damage = 5,
        GibColor = 0x7f7165,
        BoundingBox = SpecialBoundingBoxes.Smoker
    };

    public static readonly SpecialConfig Charger = new()
    {
        Type = SpecialIndices.Charger,
        Health = 600,
        RunSpeed = 250,
        Damage = 15,
        GibColor = 0x435444,
        BoundingBox = SpecialBoundingBoxes.Charger
    };

    public static readonly SpecialConfig Jockey = new()
    {
        Type = SpecialIndices.Jockey,
        Health = 325,
        RunSpeed = 250,
        Damage = 4,
        GibColor = 0xbc9e9e,
        BoundingBox = SpecialBoundingBoxes.Jockey
    };

    public static readonly SpecialConfig Spitter = new()
    {
        Type = SpecialIndices.Spitter,
        Health = 100,
        RunSpeed = 210,
        Damage = 7,
        GibColor = 0xbc9e9e,
        BoundingBox = SpecialBoundingBoxes.Spitter
    };

    public static readonly SpecialConfig Tank = new()
    {
        Type = SpecialIndices.Tank,
        Health = 6000,
        RunSpeed = 210,
        Damage = 24,
        GibColor = 0xd6896b,
        BoundingBox = SpecialBoundingBoxes.Tank
    };

    public static readonly SpecialConfig Witch = new()
    {
        Type = SpecialIndices.Witch,
        Health = 1000,
        RunSpeed = 350,
        Damage = 100,
        GibColor = 0xbcb8b8,
        BoundingBox = SpecialBoundingBoxes.Witch
    };
}