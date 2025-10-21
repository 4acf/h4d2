using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public class Bullet : Projectile
{
    private const double _speed = 200;
    private const int _color = 0xffffff;
    private double _oldXPosition;
    private double _oldYPosition;
    
    public Bullet(Level level, double directionRadians, double xPosition, double yPosition) 
        : base(level, new BoundingBox(false, 1, 1), directionRadians, xPosition, yPosition)
    {
        _oldXPosition = xPosition;
        _oldYPosition = yPosition;
    }
    
    public override void Update(double elapsedTime)
    {
        _oldXPosition = XPosition;
        _oldYPosition = YPosition;
        double timeAdjustedSpeed = _speed * elapsedTime;
        _xVelocity = Math.Cos(_directionRadians) * timeAdjustedSpeed;
        _yVelocity = Math.Sin(_directionRadians) * timeAdjustedSpeed;
        _AttemptMove();
    }

    public override void Render(Bitmap screen)
    {
        double xDifference = _oldXPosition - XPosition;
        double yDifference = _oldYPosition - YPosition;
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            screen.SetPixel((int)Math.Ceiling(XPosition + xDifference * i / steps), (int)(YPosition + yDifference * i / steps), _color);
        }
    }

    public override void RenderShadow(Bitmap screen)
    {
        double xDifference = _oldXPosition - XPosition;
        double yDifference = _oldYPosition - YPosition;
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            screen.SetPixelBlend((int)(XPosition + xDifference * i / steps), (int)(YPosition + yDifference * i / steps) - 4, 0x0, 0.9);
        }
    }

    protected override void _Collide()
    {
        base._Collide();
        Removed = true;
    }
}