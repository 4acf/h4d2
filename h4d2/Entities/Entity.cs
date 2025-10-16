using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities;

public abstract class Entity
{
    public double XPosition { get; protected set; }
    public double YPosition { get; protected set; }
    
    protected double _xVelocity;
    protected double _yVelocity;
    protected readonly Level _level;
    
    protected Entity(Level level, int xPosition, int yPosition)
    {
        XPosition = xPosition;
        YPosition = yPosition;
        _xVelocity = 0;
        _yVelocity = 0;
        _level = level;
    }
    
    public abstract void Update(double elapsedTime);
    public abstract void Render(Bitmap screen);
    public abstract void RenderShadow(Bitmap screen);
    
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

        // todo: create a bounding box around an entity for more accurate collisions
        if (xDest < 0 || yDest < Art.SpriteSize || xDest >= _level.Width - Art.SpriteSize || yDest >= _level.Height)
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