using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public class Bullet : Entity
{
    private const double _speed = 200;
    private readonly double _directionRadians;
    
    public Bullet(Level level, double directionRadians, double xPosition, double yPosition) 
        : base(level, new BoundingBox(false, 1, 1), xPosition, yPosition)
    {
        _directionRadians = directionRadians;
    }
    
    public override void Update(double elapsedTime)
    {
        double timeAdjustedSpeed = _speed * elapsedTime;
        _xVelocity = Math.Cos(_directionRadians) * timeAdjustedSpeed;
        _yVelocity = Math.Sin(_directionRadians) * timeAdjustedSpeed;
        _AttemptMove();
    }

    public override void Render(Bitmap screen)
    {
        screen.SetPixel((int)Math.Ceiling(XPosition), (int)YPosition, 0xffffff);
    }

    public override void RenderShadow(Bitmap screen)
    {
        screen.SetPixelBlend((int)Math.Ceiling(XPosition), (int)YPosition - 4, 0x0, 0.9);
    }
}