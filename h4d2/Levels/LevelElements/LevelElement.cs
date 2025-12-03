using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Levels.LevelElements;

public abstract class LevelElement : Isometric
{
    private readonly int _type;
    private const int _xOffset = H4D2Art.TileSize / 2;
    private const int _yOffset = (H4D2Art.TileSize * 3) / 4;
    
    protected LevelElement(Level level, Position position, LevelElementConfig config)
        : base(level, position)
    {
        _type = config.Type;
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = H4D2Art.Walls[_type];
        screen.Draw(bitmap, xCorrected - _xOffset, yCorrected - _yOffset);
    }
}