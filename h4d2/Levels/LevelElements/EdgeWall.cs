using H4D2.Infrastructure;

namespace H4D2.Levels.LevelElements;

public class EdgeWall : LevelElement
{
    public EdgeWall(Level level, Position position)
        : base(level, position, LevelElementConfigs.EdgeWall)
    {
        
    }
}