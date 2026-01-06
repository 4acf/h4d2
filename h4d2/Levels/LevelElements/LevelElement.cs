using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Levels.LevelElements;

public abstract class LevelElement : Isometric
{
    private readonly int _type;
    
    protected LevelElement(Level level, Position position, LevelElementConfig config)
        : base(level, position)
    {
        _type = config.Type;
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.Draw(H4D2Art.Tiles.Walls[_type], xCorrected, yCorrected);
    }
}