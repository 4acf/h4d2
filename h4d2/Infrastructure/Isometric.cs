using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Infrastructure;

public abstract class Isometric
{
    public const double ScaleX = 12.0 / 16.0;
    public const double ScaleY = 6.0 / 16.0;
    
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

    public static (double, double) ScreenSpaceToWorldSpace(double xScreenPixels, double yScreenPixels)
    {
        return (
            ((xScreenPixels / ScaleX) + (yScreenPixels / ScaleY)) / 2,
            ((yScreenPixels / ScaleY) - (xScreenPixels / ScaleX)) / 2
        );
    }

    public static (int, int) WorldSpaceToScreenSpace(double xPos, double yPos)
    {
        return (
            (int)(Math.Floor(xPos - yPos) * ScaleX),
            (int)(Math.Floor(xPos + yPos) * ScaleY)
        );
    }
    
    public void Render(H4D2BitmapCanvas screen)
    {
        int xCorrected = (int)(Math.Floor(_position.X - _position.Y) * ScaleX);
        int yCorrected = (int)(Math.Floor(_position.Y + _position.X) * ScaleY + _position.Z);
        Render(screen, xCorrected, yCorrected);
    }

    protected virtual void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        
    }

    public void RenderShadow(ShadowBitmap shadows)
    {
        int xCorrected = (int)(Math.Floor(_position.X - _position.Y) * ScaleX);
        int yCorrected = (int)(Math.Floor(_position.Y + _position.X) * ScaleY);
        RenderShadow(shadows, xCorrected, yCorrected);
    }

    protected virtual void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        
    }
}