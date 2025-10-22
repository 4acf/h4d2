namespace H4D2.Entities;

public class BoundingBox
{
    public readonly int CollisionMask;
    private readonly int _collidesWith;
    private readonly int _xOffset;
    private readonly int _yOffset;
    private readonly int _width;
    private readonly int _height;
    
    public BoundingBox(int collisionMask, int collidesWith, int xOffset, int yOffset, int width, int height)
    {
        CollisionMask = collisionMask;
        _collidesWith = collidesWith;
        _xOffset = xOffset;
        _yOffset = yOffset;
        _width = width;
        _height = height;
    }

    public BoundingBox(int collisionMask, int collidesWith, int width, int height)
    {
        CollisionMask = collisionMask;
        _collidesWith = collidesWith;
        _xOffset = 0;
        _yOffset = 0;
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

    public bool CanCollideWith(BoundingBox other)
    {
        return (_collidesWith & other.CollisionMask) == other.CollisionMask;
    }
    
    public bool IsIntersecting(BoundingBox other, double otherXPosition, double otherYPosition, double xPosition, double yPosition)
    {
        return
            other.W(otherXPosition) <= E(xPosition) &&
            other.E(otherXPosition) >= W(xPosition) &&
            other.N(otherYPosition) >= S(yPosition) &&
            other.S(otherYPosition) <= N(yPosition);
    }

    public (double, double) CenterMass(double xPosition, double yPosition)
    {
        return ((W(xPosition) + E(xPosition)) / 2, (N(yPosition) + S(yPosition)) / 2);
    }
    
}