using H4D2.Entities.Mobs.Zombies;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;

public class Bullet : Projectile
{
    private const double _speed = 200.0;
    private const int _color = 0xffffff;

    private Position _oldPosition;
    private int _piercing;
    private readonly HashSet<Zombie> _alreadyHit;
    
    public Bullet(Level level, Position position, int damage, int piercing, double directionRadians) 
        : base(level, position, ProjectileConfig.BulletBoundingBox, damage, directionRadians)
    {
        _piercing = piercing;
        _alreadyHit = new HashSet<Zombie>(piercing);
        _oldPosition = position.Copy();
    }
    
    public override void Update(double elapsedTime)
    {
        _oldPosition = _position.Copy();
        double timeAdjustedSpeed = _speed * elapsedTime;
        _velocity.X = Math.Cos(DirectionRadians) * timeAdjustedSpeed;
        _velocity.Y = Math.Sin(DirectionRadians) * timeAdjustedSpeed;
        _AttemptMove();
    }

    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        double oldXCorrected = (_oldPosition.X - _oldPosition.Y) * ScaleX;
        double oldYCorrected = ((_oldPosition.X + _oldPosition.Y) * ScaleY) + _oldPosition.Z;

        double xCorrectedDouble = (_position.X - _position.Y) * ScaleX;
        double yCorrectedDouble = ((_position.X + _position.Y) * ScaleY) + _position.Z;
        
        double xDifference = xCorrectedDouble - oldXCorrected;
        double yDifference = yCorrectedDouble - oldYCorrected;
        
        int steps = (int)(Math.Sqrt(xDifference * xDifference + yDifference * yDifference) + 1);
        for (int i = 0; i < steps; i++)
        {
            screen.SetPixel(
                (int)(xCorrectedDouble + xDifference * i / steps), 
                (int)(yCorrectedDouble + yDifference * i / steps),
                _color
            );
        }
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        double oldXCorrected = (_oldPosition.X - _oldPosition.Y) * ScaleX;
        double oldYCorrected = ((_oldPosition.X + _oldPosition.Y) * ScaleY);
        
        double xCorrectedDouble = (_position.X - _position.Y) * ScaleX;
        double yCorrectedDouble = ((_position.X + _position.Y) * ScaleY);
        
        double xDifference = xCorrectedDouble - oldXCorrected;
        double yDifference = yCorrectedDouble - oldYCorrected;
        
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
        if (entity == null || entity is not Zombie zombie)
        {
            base._Collide(entity);
            Removed = true;
            return;
        }

        if (_alreadyHit.Contains(zombie))
            return;
        
        zombie.HitBy(this);
        _alreadyHit.Add(zombie);
        _piercing--;
        if (_piercing == 0)
        {
            base._Collide(entity);
            Removed = true;
        }
    }

    protected override void _CollideWall()
    {
        _Collide(null);
    }
}