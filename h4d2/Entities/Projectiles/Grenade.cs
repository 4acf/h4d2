using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;
using Cfg = ProjectileConfig;

public class Grenade : Projectile
{
    private const double _speed = 150;
    private const int _color = 0x333333;
    private readonly int _directionIndex;

    private static readonly (int, int)[][] _sprites =
    {
        [(-1, 0), (1, 0), (-1, -1), (0, -1), (1, -1)], // E
        [(1, 0), (-1, -1), (0, -1), (1, -1), (0, -2)], // NE
        [(1, 0), (0, -1), (1, -1), (0, -2), (1, -2)],  // N
        [(1, 0), (0, -1), (1, -1), (2, -1), (1, -2)],  // NW
        [(1, 0), (2, 0), (0, -1), (1, -1), (2, -1)],   // W
        [(1, 1), (1, 0), (2, 0), (0, -1), (1, -1)],    // SW
        [(0, 1), (1, 1), (1, 0), (0, -1), (1, -1)],    // S 
        [(0, 1), (-1, 0), (1, 0), (0, -1), (1, -1)]    // SE
    };
    
    public Grenade(Level level, double directionRadians, double xPosition, double yPosition)
        : base(level, new BoundingBox(Cfg.CollisionMask, Cfg.CollidesWith, 2, 2), directionRadians, xPosition, yPosition)
    {
        _directionIndex = _ResolveDirectionIndex(directionRadians);
    }

    private static int _ResolveDirectionIndex(double directionRadians)
    {
        int index = 0;
        double degrees = MathHelpers.RadiansToDegrees(directionRadians);
        index = degrees switch
        {
            >= 337.5 or  < 22.5  => 0,
            >= 22.5  and < 67.5  => 1,
            >= 67.5  and < 112.5 => 2,
            >= 112.5 and < 157.5 => 3,
            >= 157.5 and < 202.5 => 4,
            >= 202.5 and < 247.5 => 5,
            >= 247.5 and < 292.5 => 6,
            >= 292.5 and < 337.5 => 7,
            _ => index
        };
        return index;
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
        screen.SetPixel((int)XPosition, (int)YPosition, _color);
        for (int i = 0; i < _sprites[_directionIndex].Length; i++)
        {
            int dx = _sprites[_directionIndex][i].Item1;
            int dy = _sprites[_directionIndex][i].Item2;
            screen.SetPixel((int)XPosition + dx, (int)YPosition + dy, _color);
        }
    }

    public override void RenderShadow(Bitmap screen)
    {
        screen.SetPixelBlend((int)XPosition, (int)YPosition - 4, 0x0, 0.9);
        for (int i = 0; i < _sprites[_directionIndex].Length; i++)
        {
            int dx = _sprites[_directionIndex][i].Item1;
            int dy = _sprites[_directionIndex][i].Item2;
            screen.SetPixelBlend((int)XPosition + dx, (int)YPosition + dy - 4, 0x0, 0.9);
        }
    }

    protected override void _Collide()
    {
        base._Collide();
        Removed = true;
    }
}