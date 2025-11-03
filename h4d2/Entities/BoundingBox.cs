using H4D2.Infrastructure;

namespace H4D2.Entities;

public record BoundingBoxDimensions
{
    public readonly int XWidth;
    public readonly int YWidth;
    public readonly int ZHeight;
    public readonly int SpriteSize;
    public readonly int XOffset;

    public BoundingBoxDimensions(int xWidth, int yWidth, int zHeight, int spriteSize, int xOffset = 0)
    {
        XWidth = xWidth;
        YWidth = yWidth;
        ZHeight = zHeight;
        SpriteSize = spriteSize;
        XOffset = xOffset;
    }
}

public class BoundingBox
{
    public readonly int CollisionMask;
    private readonly int _collidesWith;
    private readonly int _xWidth;
    private readonly int _yWidth;
    private readonly int _zHeight;
    private readonly int _spriteSize;
    private readonly int _xOffset;
    
    public BoundingBox(int collisionMask, int collidesWith, BoundingBoxDimensions dimensions)
    {
        CollisionMask = collisionMask;
        _collidesWith = collidesWith;
        _xWidth = dimensions.XWidth;
        _yWidth = dimensions.YWidth;
        _zHeight = dimensions.ZHeight;
        _spriteSize = dimensions.SpriteSize;
        _xOffset = dimensions.XOffset;
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
    
    public bool IsIntersecting(Entity other, ReadonlyPosition position)
    {
        ReadonlyPosition otherPosition = other.Position;
        
        bool isXYPlaneIntersecting =
            other.BoundingBox.W(otherPosition.X) <= E(position.X) &&
            other.BoundingBox.E(otherPosition.X) >= W(position.X) &&
            other.BoundingBox.N(otherPosition.Y) >= S(position.Y) &&
            other.BoundingBox.S(otherPosition.Y) <= N(position.Y);
        
        bool isZIntersecting =
            other.BoundingBox.Bottom(otherPosition.Z) <= Top(position.Z) &&
            other.BoundingBox.Top(otherPosition.Z) >= Bottom(position.Z);

        return isXYPlaneIntersecting && isZIntersecting;
    }

    public ReadonlyPosition CenterMass(ReadonlyPosition position)
    {
        return new ReadonlyPosition(
            (W(position.X) + E(position.X)) / 2,
            (N(position.Y) + S(position.Y)) / 2,
            (position.Z + _zHeight) / 2.0
        );
    }

    public ReadonlyPosition FootPosition(ReadonlyPosition position)
    {
        return new ReadonlyPosition(
            (W(position.X) + E(position.X)) / 2,
            S(position.Y),
            position.Z
        );
    }
}