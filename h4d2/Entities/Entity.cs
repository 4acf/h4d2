using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities;

public abstract class Entity : Isometric
{
    public readonly BoundingBox BoundingBox;
    public ReadonlyPosition CenterMass => BoundingBox.CenterMass(Position); 
    public ReadonlyPosition FootPosition => BoundingBox.FootPosition(Position);
    
    protected double _xVelocity;
    protected double _yVelocity;
    protected double _zVelocity;
    
    protected Entity(Level level, Position position, BoundingBox boundingBox)
        : base(level, position)
    {
        BoundingBox = boundingBox;
        _xVelocity = 0;
        _yVelocity = 0;
        _zVelocity = 0;
    }
    
    public abstract void Update(double elapsedTime);

    public bool IsIntersecting(Entity other, ReadonlyPosition position) =>
        BoundingBox.IsIntersecting(other, position);
    
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
        if (w < -Level.Padding) 
            return true;
        
        double s = BoundingBox.S(yPosition);
        if(s < -Level.Padding) 
            return true;
        
        double e = BoundingBox.E(xPosition);
        if (e >= _level.Width + Level.Padding) 
            return true;
        
        double n = BoundingBox.N(yPosition);
        if (n >= _level.Height + Level.Padding) 
            return true;

        if (zPosition < 0)
            return true;
        
        return false;
    }
    
    private void _Move(double xComponent, double yComponent, double zComponent)
    {
        double xDest = _position.X + xComponent;
        double yDest = _position.Y + yComponent;
        double zDest = _position.Z + zComponent;

        if (_IsOutOfLevelBounds(xDest, yDest, zDest))
        {
            if (zDest < 0) _position.Z = 0;
            _Collide(null);
            return;
        }

        Entity? collidingEntity = _level.GetFirstCollidingEntity(this, Position);
        if (collidingEntity != null)
        {
            _Collide(collidingEntity);
            return;
        }

        _position.X = xDest;
        _position.Y = yDest;
        _position.Z = zDest;
    }
    
    protected virtual void _Collide(Entity? entity)
    {
        _xVelocity = 0;
        _yVelocity = 0;
        _zVelocity = 0;
    }
}