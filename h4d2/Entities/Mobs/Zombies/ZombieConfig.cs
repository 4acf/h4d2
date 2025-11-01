namespace H4D2.Entities.Mobs.Zombies;

public class ZombieConfig : MobConfig
{
    public required int Damage { get; init; }
}

public static class ZombieCollision
{
    public const int CollisionMask = 0b10;
    public const int CollidesWith = 0b100;
}