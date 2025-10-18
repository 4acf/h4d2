namespace H4D2.Entities.Mobs.Specials;

public static class SpecialConfig
{
    public const int Hunter = 0;
    public const int HunterHealth = 250;
    public const int HunterRunSpeed = 250;
    public static readonly BoundingBox HunterBoundingBox = new(true, 4, 7, 8, 9);

    public const int Boomer = 1;
    public const int BoomerHealth = 50;
    public const int BoomerRunSpeed = 175;
    public static readonly BoundingBox BoomerBoundingBox = new(true, 3, 7, 10, 9);

    public const int Smoker = 2;
    public const int SmokerHealth = 250;
    public const int SmokerRunSpeed = 210;
    public static readonly BoundingBox SmokerBoundingBox = new(true, 6, 4, 8, 12);

    public const int Charger = 3;
    public const int ChargerHealth = 600;
    public const int ChargerRunSpeed = 250;
    public static readonly BoundingBox ChargerBoundingBox = new(true, 3, 4, 10, 12);

    public const int Jockey = 4;
    public const int JockeyHealth = 325;
    public const int JockeyRunSpeed = 250;
    public static readonly BoundingBox JockeyBoundingBox = new(true, 6, 11, 8, 5);

    public const int Spitter = 5;
    public const int SpitterHealth = 100;
    public const int SpitterRunSpeed = 210;
    public static readonly BoundingBox SpitterBoundingBox = new(true, 6, 4, 8, 12);

    public const int Tank = 6;
    public const int TankHealth = 6000;
    public const int TankRunSpeed = 210;
    public static readonly BoundingBox TankBoundingBox = new(true, 1, 4, 14, 12);

    public const int Witch = 7;
    public const int WitchHealth = 1000;
    public const int WitchRunSpeed = 300;
    public static readonly BoundingBox WitchBoundingBox = new(true, 4, 6, 8, 10);
}