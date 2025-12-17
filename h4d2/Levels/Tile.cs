namespace H4D2.Levels;

public enum TileType
{
    Floor,
    Wall,
    ZombieWall,
    EdgeWall,
    SurvivorFloor
}

public readonly record struct Tile
{
    public readonly int X;
    public readonly int Y;
    
    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }
}