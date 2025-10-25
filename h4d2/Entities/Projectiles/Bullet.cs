using H4D2.Entities.Mobs.Zombies;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;
using Cfg = ProjectileConfig;

public class Bullet : Projectile
{
    private const double _speed = 200;
    private const int _color = 0xffffff;
    private double _oldXPosition;
    private double _oldYPosition;
    private double _oldZPosition;
    
    public Bullet(Level level, double directionRadians, double xPosition, double yPosition, double zPosition, int damage) 
        : base(level, new BoundingBox(Cfg.CollisionMask, Cfg.CollidesWith, 1, 1, 1, 0), directionRadians, xPosition, yPosition, zPosition, damage)
    {
        _oldXPosition = xPosition;
        _oldYPosition = yPosition;
        _oldZPosition = zPosition;
    }
    
    public override void Update(double elapsedTime)
    {
        _oldXPosition = XPosition;
        _oldYPosition = YPosition;
        _oldZPosition = ZPosition;
        double timeAdjustedSpeed = _speed * elapsedTime;
        _xVelocity = Math.Cos(_directionRadians) * timeAdjustedSpeed;
        _yVelocity = Math.Sin(_directionRadians) * timeAdjustedSpeed;
        _AttemptMove();
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        double oldYCorrected = _oldYPosition + _oldZPosition;
        double yCorrectedDouble = YPosition + ZPosition;
        
        double xDifference = XPosition - _oldXPosition;
        double yDifference = yCorrectedDouble - oldYCorrected;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            screen.SetPixel((int)(XPosition + xDifference * i / steps), (int)(yCorrectedDouble + yDifference * i / steps), _color);
        }
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        double xCorrectedDouble = XPosition;
        double yCorrectedDouble = YPosition;
        
        double xDifference = xCorrectedDouble - _oldXPosition;
        double yDifference = yCorrectedDouble - _oldYPosition;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            screen.SetPixelBlend((int)(xCorrectedDouble + xDifference * i / steps), (int)(yCorrectedDouble + yDifference * i / steps), 0x0, 0.9);
        }
    }

    protected override void _Collide(Entity? entity)
    {
        base._Collide(entity);
        Removed = true;
        if (entity == null || entity is not Zombie zombie)
            return;
        zombie.HitBy(this);
    }
}