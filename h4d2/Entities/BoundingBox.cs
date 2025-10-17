namespace H4D2.Entities;

public class BoundingBox
{
    public readonly bool IsBlocking;
    private readonly int _xOffset;
    private readonly int _yOffset;
    private readonly int _width;
    private readonly int _height;
    
    public BoundingBox(bool isBlocking, int xOffset, int yOffset, int width, int height)
    {
        IsBlocking = isBlocking;
        _xOffset = xOffset;
        _yOffset = yOffset;
        _width = width;
        _height = height;
    }
    
    public double N(double yPosition) => yPosition - _yOffset;
    public double E(double xPosition) => xPosition + _xOffset + _width;
    public double S(double yPosition) => yPosition - _yOffset - _height;
    public double W(double xPosition) => xPosition + _xOffset;
    public (double, double) SW(double xPosition, double yPosition) => (W(xPosition), S(yPosition));
    public (double, double) NW(double xPosition, double yPosition) => (W(xPosition), N(yPosition));
    public (double, double) SE(double xPosition, double yPosition) => (E(xPosition), S(yPosition));
    public (double, double) NE(double xPosition, double yPosition) => (E(xPosition), N(yPosition));

    public static bool IsIntersecting((double, double) point, double n, double e, double s, double w)
    {
        double x = point.Item1;
        double y = point.Item2;
        
        return 
            x >= w &&
            x <= e &&
            y >= s &&
            y <= n;
    }
}