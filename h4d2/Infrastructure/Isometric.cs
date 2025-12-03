using H4D2.Levels;

namespace H4D2.Infrastructure;

public abstract class Isometric
{
    protected const double _scaleX = 3.0 / 4.0;
    protected const double _scaleY = 3.0 / 8.0;
    
    protected readonly Level _level;
    protected readonly Position _position;
    protected readonly Velocity _velocity;
    public ReadonlyPosition Position => _position.ReadonlyCopy(); 
    public bool Removed { get; protected set; }
    public bool IsOnGround => _position.Z == 0;
    
    protected Isometric(Level level, Position position)
    {
        _level = level;
        _position = position;
        _velocity = new Velocity();
        Removed = false;
    }
    
    public void Render(Bitmap screen)
    {
        int xCorrected = (int)(Math.Floor(_position.X - _position.Y) * _scaleX);
        int yCorrected = (int)(Math.Floor(_position.Y + _position.X) * _scaleY - _position.Z);
        Render(screen, xCorrected, yCorrected);
    }

    protected virtual void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        
    }

    public void RenderShadow(ShadowBitmap shadows)
    {
        int xCorrected = (int)(Math.Floor(_position.X - _position.Y) * _scaleX);
        int yCorrected = (int)(Math.Floor(_position.Y + _position.X) * _scaleY);
        RenderShadow(shadows, xCorrected, yCorrected);
    }

    protected virtual void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        
    }
}