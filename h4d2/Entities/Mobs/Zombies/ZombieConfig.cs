namespace H4D2.Entities.Mobs.Zombies;

public class ZombieConfig
{
    public required int Health { get; init; }
    public required int RunSpeed { get; init; }
    public required int Damage { get; init; }
    public required int GibColor { get; init; }
    public required BoundingBox BoundingBox { get; init; }
}

public static class ZombieCollision
{
    public const int CollisionMask = 0b10;
    public const int CollidesWith = 0b100;
}