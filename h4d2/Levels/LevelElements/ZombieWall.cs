using H4D2.Infrastructure;

namespace H4D2.Levels.LevelElements;

public class ZombieWall : LevelElement
{
    public ZombieWall(Level level, Position position)
        : base(level, position, LevelElementConfigs.ZombieWall)
    {
        
    }
}