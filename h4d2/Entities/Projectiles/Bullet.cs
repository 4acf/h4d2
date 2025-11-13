using H4D2.Entities.Mobs.Zombies;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public class Bullet : Projectile
{
    private const double _speed = 200.0;
    private const int _color = 0xffffff;

    private Position _oldPosition;
    
    public Bullet(Level level, Position position, int damage, double directionRadians) 
        : base(level, position, ProjectileConfig.BulletBoundingBox, damage, directionRadians)
    {
        _oldPosition = position.Copy();
    }
    
    public override void Update(double elapsedTime)
    {
        _oldPosition = _position.Copy();
        double timeAdjustedSpeed = _speed * elapsedTime;
        _velocity.X = Math.Cos(_directionRadians) * timeAdjustedSpeed;
        _velocity.Y = Math.Sin(_directionRadians) * timeAdjustedSpeed;
        _AttemptMove();
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        double oldYCorrected = _oldPosition.Y + _oldPosition.Z;
        double yCorrectedDouble = _position.Y + _position.Z;
        
        double xDifference = _position.X - _oldPosition.X;
        double yDifference = yCorrectedDouble - oldYCorrected;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            screen.SetPixel((int)(_position.X + xDifference * i / steps), (int)(yCorrectedDouble + yDifference * i / steps), _color);
        }
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        double xCorrectedDouble = _position.X;
        double yCorrectedDouble = _position.Y;
        
        double xDifference = xCorrectedDouble - _oldPosition.X;
        double yDifference = yCorrectedDouble - _oldPosition.Y;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            int x = (int)(xCorrectedDouble + xDifference * i / steps);
            int y = (int)(yCorrectedDouble + yDifference * i / steps);
            shadows.SetPixel(x, y);
        }
    }

    protected override void _Collide(Entity? entity)
    {
        base._Collide(entity);
        if (entity == null || entity is not Zombie zombie)
        {
            Removed = true;
            return;
        }
        zombie.HitBy(this);
        Removed = true;
    }
}