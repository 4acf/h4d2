using H4D2.Levels;

namespace H4D2.Infrastructure;

public abstract class Isometric
{
    protected Level _level;
    public double XPosition { get; protected set; }
    public double YPosition { get; protected set; }
    public double ZPosition { get; protected set; }
    public bool Removed { get; protected set; }
    public bool IsOnGround => ZPosition == 0;
    
    protected Isometric(Level level, double xPosition, double yPosition, double zPosition)
    {
        _level = level;
        XPosition = xPosition;
        YPosition = yPosition;
        ZPosition = zPosition;
        Removed = false;
    }
    
    public void Render(Bitmap screen)
    {
        int xCorrected = (int)XPosition;
        int yCorrected = (int)(YPosition + ZPosition);
        Render(screen, xCorrected, yCorrected);
    }
    protected abstract void Render(Bitmap screen, int xCorrected, int yCorrected);

    public void RenderShadow(Bitmap screen)
    {
        int xCorrected = (int)XPosition;
        int yCorrected = (int)YPosition;
        RenderShadow(screen, xCorrected, yCorrected);
    }
    protected abstract void RenderShadow(Bitmap screen, int xCorrected, int yCorrected);
}