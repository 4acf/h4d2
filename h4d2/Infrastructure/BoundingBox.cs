using H4D2.Entities;
using H4D2.Infrastructure.H4D2;

namespace H4D2.Infrastructure;

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
    public readonly CollisionGroup CollisionGroup;
    private readonly int _xWidth;
    private readonly int _yWidth;
    private readonly int _zHeight;
    private readonly int _spriteSize;
    private readonly int _xOffset;
    
    public BoundingBox(CollisionGroup collisionGroup, BoundingBoxDimensions dimensions)
    {
        CollisionGroup = collisionGroup;
        _xWidth = dimensions.XWidth;
        _yWidth = dimensions.YWidth;
        _zHeight = dimensions.ZHeight;
        _spriteSize = dimensions.SpriteSize;
        _xOffset = dimensions.XOffset;
    }
    
    public double N(double xPosition, double yPosition)
    {
        (double, double) nw = ScreenNW(xPosition, yPosition);
        (double, double) ne = ScreenNE(xPosition, yPosition);
        (double, double) avg = ((nw.Item1 + ne.Item1) / 2, (nw.Item2 + ne.Item2) / 2);
        return avg.Item2;
    }

    public double E(double xPosition, double yPosition)
    {
        (double, double) ne = ScreenNE(xPosition, yPosition);
        (double, double) se = ScreenSE(xPosition, yPosition);
        (double, double) avg = ((ne.Item1 + se.Item1) / 2, (ne.Item2 + se.Item2) / 2);
        return avg.Item1;
    }

    public double S(double xPosition, double yPosition)
    {
        (double, double) sw = ScreenNW(xPosition, yPosition);
        (double, double) se = ScreenNE(xPosition, yPosition);
        (double, double) avg = ((sw.Item1 + se.Item1) / 2, (sw.Item2 + se.Item2) / 2);
        return avg.Item2;
    }

    public double W(double xPosition, double yPosition)
    {
        (double, double) nw = ScreenNW(xPosition, yPosition);
        (double, double) sw = ScreenSW(xPosition, yPosition);
        (double, double) avg = ((nw.Item1 + sw.Item1) / 2, (nw.Item2 + sw.Item2) / 2);
        return avg.Item1;
    }

    public (double, double) NW(double xPosition, double yPosition) 
        => (W(xPosition, yPosition), N(xPosition, yPosition));
    public (double, double) NE(double xPosition, double yPosition)
        => (E(xPosition, yPosition), N(xPosition, yPosition));
    public (double, double) SE(double xPosition, double yPosition)
        => (E(xPosition, yPosition), S(xPosition, yPosition));
    public (double, double) SW(double xPosition, double yPosition)
        => (W(xPosition, yPosition), S(xPosition, yPosition));
    
    private static (double, double) _Corner(double xPosition, double yPosition, double xScreenOffs, double yScreenOffs)
    {
        (double, double) offsets = Isometric.ScreenSpaceToWorldSpace(xScreenOffs, yScreenOffs);
        return (xPosition + offsets.Item1, yPosition + offsets.Item2);
    }
    public (double, double) ScreenSW(double xPosition, double yPosition)
        => _Corner(xPosition, yPosition, _xOffset, -_spriteSize);

    public (double, double) ScreenNW(double xPosition, double yPosition)
        => _Corner(xPosition, yPosition, _xOffset, -_spriteSize + _yWidth);

    public (double, double) ScreenSE(double xPosition, double yPosition)
        => _Corner(xPosition, yPosition, _xOffset + _xWidth, -_spriteSize);

    public (double, double) ScreenNE(double xPosition, double yPosition)
        => _Corner(xPosition, yPosition, _xOffset + _xWidth, -_spriteSize + _yWidth);
    
    public double Top(double zPosition) => zPosition + _zHeight;
    public double Bottom(double zPosition) => zPosition;
    
    public bool CanCollideWith(CollisionManager<CollisionGroup> collisionManager, BoundingBox other)
    {
        return collisionManager.CanCollideWith(CollisionGroup, other.CollisionGroup);
    }
    
    public bool IsIntersecting(Entity other, ReadonlyPosition position)
    {
        ReadonlyPosition otherPosition = other.Position;
        
        bool isXYPlaneIntersecting =
            other.BoundingBox.W(otherPosition.X, otherPosition.Y) <= E(position.X, position.Y) &&
            other.BoundingBox.E(otherPosition.X, otherPosition.Y) >= W(position.X, position.Y) &&
            other.BoundingBox.N(otherPosition.X, otherPosition.Y) >= S(position.X, position.Y) &&
            other.BoundingBox.S(otherPosition.X, otherPosition.Y) <= N(position.X, position.Y);
        
        bool isZIntersecting =
            other.BoundingBox.Bottom(otherPosition.Z) <= Top(position.Z) &&
            other.BoundingBox.Top(otherPosition.Z) >= Bottom(position.Z);

        return isXYPlaneIntersecting && isZIntersecting;
    }

    public ReadonlyPosition CenterMass(ReadonlyPosition position)
    {
        return new ReadonlyPosition(
            (W(position.X, position.Y) + E(position.X, position.X)) / 2,
            (N(position.X, position.Y) + S(position.X, position.Y)) / 2,
            (position.Z + _zHeight) / 2.0
        );
    }

    public ReadonlyPosition FootPosition(ReadonlyPosition position)
    {
        return new ReadonlyPosition(
            (W(position.X, position.Y) + E(position.X, position.Y)) / 2,
            S(position.X, position.Y),
            position.Z
        );
    }
}