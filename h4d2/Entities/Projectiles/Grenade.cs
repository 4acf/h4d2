using H4D2.Entities.Mobs.Zombies;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles;

namespace H4D2.Entities.Projectiles;
using Cfg = ProjectileConfig;

public class Grenade : Projectile
{
    public const double SplashRadius = 15.0;
    private const double _speed = 150;
    private const int _color = 0x333333;
    private const int _numSmokeParticlesPerSecond = 120;
    private const double _gravity = 0.15;
    
    private readonly int _directionIndex;
    
    private static readonly (int, int)[][] _sprites =
    {
        [(-1,  0), ( 1,  0), (-1, -1), ( 0, -1), ( 1, -1)], // E
        [( 1,  0), (-1, -1), ( 0, -1), ( 1, -1), ( 0, -2)], // NE
        [( 1,  0), ( 0, -1), ( 1, -1), ( 0, -2), ( 1, -2)], // N
        [( 1,  0), ( 0, -1), ( 1, -1), ( 2, -1), ( 1, -2)], // NW
        [( 1,  0), ( 2,  0), ( 0, -1), ( 1, -1), ( 2, -1)], // W
        [( 1,  1), ( 1,  0), ( 2,  0), ( 0, -1), ( 1, -1)], // SW
        [( 0,  1), ( 1,  1), ( 1,  0), ( 0, -1), ( 1, -1)], // S
        [( 0,  1), (-1,  0), ( 1,  0), ( 0, -1), ( 1, -1)]  // SE
    };
    
    public Grenade(Level level, Position position, int damage, double directionRadians)
        : base(level, position, Cfg.GrenadeBoundingBox, damage, directionRadians)
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
        for (int i = 0; i < _numSmokeParticlesPerSecond * elapsedTime; i++)
        {
            var smoke = new Smoke(_level, _position.Copy(), _xVelocity, _yVelocity);
            _level.AddParticle(smoke);
        }
        double timeAdjustedSpeed = _speed * elapsedTime;
        _xVelocity = Math.Cos(_directionRadians) * timeAdjustedSpeed;
        _yVelocity = Math.Sin(_directionRadians) * timeAdjustedSpeed;
        _zVelocity -= _gravity * elapsedTime;
        _AttemptMove();
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.SetPixel(xCorrected, yCorrected, _color);
        for (int i = 0; i < _sprites[_directionIndex].Length; i++)
        {
            int dx = _sprites[_directionIndex][i].Item1;
            int dy = _sprites[_directionIndex][i].Item2;
            screen.SetPixel(xCorrected + dx, yCorrected + dy, _color);
        }
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.SetPixelBlend(xCorrected, yCorrected, Art.ShadowColor, Art.ShadowBlend);
        for (int i = 0; i < _sprites[_directionIndex].Length; i++)
        {
            int dx = _sprites[_directionIndex][i].Item1;
            int dy = _sprites[_directionIndex][i].Item2;
            screen.SetPixelBlend(xCorrected + dx, yCorrected + dy, Art.ShadowColor, Art.ShadowBlend);
        }
    }

    protected override void _Collide(Entity? entity)
    {
        // currently this will double grenade damage on a direct hit (Explode + HitBy damage will stack)
        // im inclined to keep this for balancing reasons
        base._Collide(entity);
        _level.Explode(this);
        if (entity == null || entity is not Zombie zombie)
        {
            Removed = true;
            return;
        }
        zombie.HitBy(this);
        Removed = true;
    }
}