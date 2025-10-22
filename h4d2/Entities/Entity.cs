using H4D2.Entities.Mobs;
using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities;

public abstract class Entity
{
    protected readonly Level _level;
    public double XPosition { get; private set; }
    public double YPosition { get; private set; }
    public readonly BoundingBox BoundingBox;
    public bool Removed { get; protected set; }
    
    protected double _xVelocity;
    protected double _yVelocity;
    
    protected Entity(Level level, BoundingBox boundingBox, double xPosition, double yPosition)
    {
        _level = level;
        XPosition = xPosition;
        YPosition = yPosition;
        BoundingBox = boundingBox;
        Removed = false;
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
        return BoundingBox.IsIntersecting(other.BoundingBox, other.XPosition, other.YPosition, xPosition, yPosition);
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

        if (IsOutOfLevelBounds(xDest, yDest))
        {
            _Collide(null);
            return;
        }

        Entity? collidingEntity = _level.GetFirstCollidingEntity(this, xDest, yDest);
        if (collidingEntity != null)
        {
            _Collide(collidingEntity);
            return;
        }

        XPosition = xDest;
        YPosition = yDest;
    }
    
    protected virtual void _Collide(Entity? entity)
    {
        _xVelocity = 0;
        _yVelocity = 0;
    }
}