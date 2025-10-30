using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities;

public abstract class Entity : Isometric
{
    public readonly BoundingBox BoundingBox;
    public (double, double, double) CenterMass => BoundingBox.CenterMass(XPosition, YPosition, ZPosition); 
    
    protected double _xVelocity;
    protected double _yVelocity;
    protected double _zVelocity;
    
    protected Entity(Level level, BoundingBox boundingBox, double xPosition, double yPosition, double zPosition)
        : base(level, xPosition, yPosition, zPosition)
    {
        BoundingBox = boundingBox;
        _xVelocity = 0;
        _yVelocity = 0;
        _zVelocity = 0;
    }
    
    public abstract void Update(double elapsedTime);
    
    public bool IsIntersecting(Entity other, double xPosition, double yPosition, double zPosition)
    {
        return BoundingBox.IsIntersecting(other, xPosition, yPosition, zPosition);
    }
    
    protected void _AttemptMove()
    {
        int steps = (int)(Math.Sqrt(_xVelocity * _xVelocity + _yVelocity * _yVelocity + _zVelocity * _zVelocity) + 1);
        for (int i = 0; i < steps; i++)
        {
            _Move(_xVelocity / steps, 0, 0);
            _Move(0,_yVelocity / steps, 0);
            _Move(0, 0, _zVelocity / steps);
        }
    }

    private bool _IsOutOfLevelBounds(double xPosition, double yPosition, double zPosition)
    {
        double w = BoundingBox.W(xPosition);
        if (w < 0) 
            return true;
        
        double s = BoundingBox.S(yPosition);
        if(s < -16) 
            return true;
        
        double e = BoundingBox.E(xPosition);
        if (e >= _level.Width) 
            return true;
        
        double n = BoundingBox.N(yPosition);
        if (n >= _level.Height) 
            return true;

        if (zPosition < 0)
            return true;
        
        return false;
    }
    
    private void _Move(double xComponent, double yComponent, double zComponent)
    {
        double xDest = XPosition + xComponent;
        double yDest = YPosition + yComponent;
        double zDest = ZPosition + zComponent;

        if (_IsOutOfLevelBounds(xDest, yDest, zDest))
        {
            if (zDest < 0) ZPosition = 0;
            _Collide(null);
            return;
        }

        Entity? collidingEntity = _level.GetFirstCollidingEntity(this, xDest, yDest, zDest);
        if (collidingEntity != null)
        {
            _Collide(collidingEntity);
            return;
        }

        XPosition = xDest;
        YPosition = yDest;
        ZPosition = zDest;
    }
    
    protected virtual void _Collide(Entity? entity)
    {
        _xVelocity = 0;
        _yVelocity = 0;
        _zVelocity = 0;
    }
}