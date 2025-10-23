namespace H4D2.Infrastructure;

public abstract class Isometric
{
    public const double XScale = 3.0 / 4.0;
    public const double YScale = 3.0 / 8.0;

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
        int xCorrected = (int)(Math.Floor((XPosition - YPosition) * XScale));
        int yCorrected = (int)(Math.Floor((YPosition + XPosition) * YScale - ZPosition));
        Render(screen, xCorrected, yCorrected);
    }
    protected abstract void Render(Bitmap screen, int xCorrected, int yCorrected);

    public void RenderShadow(Bitmap screen)
    {
        int xCorrected = (int)(Math.Floor((XPosition - YPosition) * XScale));
        int yCorrected = (int)(Math.Floor((YPosition + XPosition) * YScale - ZPosition));
        RenderShadow(screen, xCorrected, yCorrected);
    }
    protected abstract void RenderShadow(Bitmap screen, int xCorrected, int yCorrected);
}