namespace H4D2.Entities.Mobs;

public class MobConfig
{
    public required int Health { get; init; }
    public required int RunSpeed { get; init; }
    public required int GibColor { get; init; }
    public required BoundingBox BoundingBox { get; init; }
}