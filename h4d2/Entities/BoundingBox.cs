namespace H4D2.Entities;

public class BoundingBox
{
    public readonly int CollisionMask;
    private readonly int _collidesWith;
    private readonly int _xOffset;
    private readonly int _xWidth;
    private readonly int _yWidth;
    private readonly int _zHeight;
    private readonly int _spriteSize;
    
    public BoundingBox(int collisionMask, int collidesWith, int xOffset, int xWidth, int yWidth, int zHeight, int spriteSize)
    {
        CollisionMask = collisionMask;
        _collidesWith = collidesWith;
        _xOffset = xOffset;
        _xWidth = xWidth;
        _yWidth = yWidth;
        _zHeight = zHeight;
        _spriteSize = spriteSize;
    }

    public BoundingBox(int collisionMask, int collidesWith, int xWidth, int yWidth, int zHeight, int spriteSize)
    {
        CollisionMask = collisionMask;
        _collidesWith = collidesWith;
        _xOffset = 0;
        _xWidth = xWidth;
        _yWidth = yWidth;
        _zHeight = zHeight;
        _spriteSize = spriteSize;
    }
    
    public double N(double yPosition) => yPosition - _spriteSize + _yWidth;
    public double E(double xPosition) => xPosition + _xOffset + _xWidth;
    public double S(double yPosition) => yPosition - _spriteSize;
    public double W(double xPosition) => xPosition + _xOffset;
    public (double, double) SW(double xPosition, double yPosition) => (W(xPosition), S(yPosition));
    public (double, double) NW(double xPosition, double yPosition) => (W(xPosition), N(yPosition));
    public (double, double) SE(double xPosition, double yPosition) => (E(xPosition), S(yPosition));
    public (double, double) NE(double xPosition, double yPosition) => (E(xPosition), N(yPosition));
    public double Top(double zPosition) => zPosition + _zHeight;
    public double Bottom(double zPosition) => zPosition;
    
    public bool CanCollideWith(BoundingBox other)
    {
        return (_collidesWith & other.CollisionMask) == other.CollisionMask;
    }
    
    public bool IsIntersecting(Entity other, double xPosition, double yPosition, double zPosition)
    {
        bool isXYPlaneIntersecting =
            other.BoundingBox.W(other.XPosition) <= E(xPosition) &&
            other.BoundingBox.E(other.XPosition) >= W(xPosition) &&
            other.BoundingBox.N(other.YPosition) >= S(yPosition) &&
            other.BoundingBox.S(other.YPosition) <= N(yPosition);
        
        bool isZIntersecting =
            other.BoundingBox.Bottom(other.ZPosition) <= Top(zPosition) &&
            other.BoundingBox.Top(other.ZPosition) >= Bottom(zPosition);

        return isXYPlaneIntersecting && isZIntersecting;
    }

    public (double, double, double) CenterMass(double xPosition, double yPosition, double zPosition)
    {
        return ((W(xPosition) + E(xPosition)) / 2, (N(yPosition) + S(yPosition)) / 2, (zPosition + _zHeight) / 2.0);
    }
    
}