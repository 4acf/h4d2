using H4D2.Infrastructure;

namespace H4D2.Entities.Mobs.Survivors;

public static class SurvivorConfig
{
    public const int Coach = 0;
    public const int Nick = 1;
    public const int Ellis = 2;
    public const int Rochelle = 3;
    public const int Bill = 4;
    public const int Francis = 5;
    public const int Louis = 6;
    public const int Zoey = 7;
    
    public static readonly BoundingBox BoundingBox = new(0b100, 0b10, 7, 2, 2, 10, Art.SpriteSize);
    public const int DefaultHealth = 100;
    public const int IncappedHealth = 300;
    public const int RunSpeed = 300;
    public const int LimpSpeed = 150;
    public const int WalkSpeed = 85;
    public const int AdrenalineRunSpeed = 260;
    public const int TempHealthDecayIntervalSeconds = 4;
    public const double IncappedHealthDecayIntervalSeconds = 1.0/3.0;
}