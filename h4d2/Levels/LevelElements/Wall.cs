using H4D2.Infrastructure;

namespace H4D2.Levels.LevelElements;

public class Wall : LevelElement
{
    public Wall(Level level, Position position)
        : base(level, position, LevelElementConfigs.Wall)
    {
        
    }
}