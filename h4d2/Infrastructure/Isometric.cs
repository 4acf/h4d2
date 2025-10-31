using H4D2.Levels;

namespace H4D2.Infrastructure;

public abstract class Isometric
{
    protected Level _level;
    protected Position _position;
    public ReadonlyPosition Position => _position.ReadonlyCopy(); 
    public bool Removed { get; protected set; }
    public bool IsOnGround => _position.Z == 0;
    
    protected Isometric(Level level, Position position)
    {
        _level = level;
        _position = position;
        Removed = false;
    }
    
    public void Render(Bitmap screen)
    {
        int xCorrected = (int)_position.X;
        int yCorrected = (int)(_position.Y + _position.Z);
        Render(screen, xCorrected, yCorrected);
    }

    protected virtual void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        
    }

    public void RenderShadow(Bitmap screen)
    {
        int xCorrected = (int)_position.X;
        int yCorrected = (int)_position.Y;
        RenderShadow(screen, xCorrected, yCorrected);
    }

    protected virtual void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        
    }
}