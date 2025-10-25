namespace H4D2.Infrastructure;

public abstract class Isometric
{
    public double XPosition { get; protected set; }
    public double YPosition { get; protected set; }
    public double ZPosition { get; protected set; }
    public bool Removed { get; protected set; }
    
    protected Isometric(double xPosition, double yPosition, double zPosition)
    {
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