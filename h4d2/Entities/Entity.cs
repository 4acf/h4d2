using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities;

public abstract class Entity
{
    protected readonly Level _level;
    public double XPosition { get; protected set; }
    public double YPosition { get; protected set; }
    public readonly BoundingBox BoundingBox;
    
    protected double _xVelocity;
    protected double _yVelocity;
    
    protected Entity(Level level, BoundingBox boundingBox, int xPosition, int yPosition)
    {
        _level = level;
        XPosition = xPosition;
        YPosition = yPosition;
        BoundingBox = boundingBox;
        _xVelocity = 0;
        _yVelocity = 0;
    }
    
    public abstract void Update(double elapsedTime);
    public abstract void Render(Bitmap screen);
    public abstract void RenderShadow(Bitmap screen);

    public bool IsOutOfLevelBounds(double xPosition, double yPosition)
    {
        double w = BoundingBox.W(xPosition);
        if (w < 0) 
            return true;
        
        double s = BoundingBox.S(yPosition);
        if(s < 0) 
            return true;
        
        double e = BoundingBox.E(xPosition);
        if (e >= _level.Width) 
            return true;
        
        double n = BoundingBox.N(yPosition);
        if (n >= _level.Height) 
            return true;

        return false;
    }
    
    public bool IsIntersecting(Entity other, double xPosition, double yPosition)
    {
        double otherN = other.BoundingBox.N(other.YPosition);
        double otherE = other.BoundingBox.E(other.XPosition);
        double otherS = other.BoundingBox.S(other.YPosition);
        double otherW = other.BoundingBox.W(other.XPosition);
        
        (double, double) point = BoundingBox.SW(xPosition, yPosition);
        if (BoundingBox.IsIntersecting(point, otherN, otherE, otherS, otherW))
            return true;
        
        point = BoundingBox.NW(xPosition, yPosition);
        if (BoundingBox.IsIntersecting(point, otherN, otherE, otherS, otherW))
            return true;
        
        point = BoundingBox.SE(xPosition, yPosition);
        if (BoundingBox.IsIntersecting(point, otherN, otherE, otherS, otherW))
            return true;
        
        point =  BoundingBox.NE(xPosition, yPosition);
        if (BoundingBox.IsIntersecting(point, otherN, otherE, otherS, otherW))
            return true;
        
        return false;
    }
    
    protected void _AttemptMove()
    {
        int steps = (int)(Math.Sqrt(_xVelocity * _xVelocity + _yVelocity * _yVelocity) + 1);
        for (int i = 0; i < steps; i++)
        {
            _Move(_xVelocity / steps, 0);
            _Move(0,_yVelocity / steps);
        }
    }

    private void _Move(double xComponent, double yComponent)
    {
        double xDest = XPosition + xComponent;
        double yDest = YPosition + yComponent;

        if (IsOutOfLevelBounds(xDest, yDest) || 
            _level.ContainsBlockingEntity(this, xDest, yDest))
        {
            _Collide();
            return;
        }

        XPosition = xDest;
        YPosition = yDest;
    }
    
    private void _Collide()
    {
        _xVelocity = 0;
        _yVelocity = 0;
    }
}