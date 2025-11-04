using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles;
using Cfg = ProjectileConfig;

public class PipeBombProjectile : Projectile
{
    public const double SplashRadius = 30.0;
    private const double _maxLifetime = 6.0;
    private const double _bounce = 0.6;
    private const int _damage = 250;
    private const double _frameDuration = 1.0 / 8.0;
    private const double _startingZVelocity = 1.0;
    private const double _gravity = 2.2;
    private const double _drag = 0.999;
    private const double _groundFriction = 0.6;
    
    private double _lifetimeSecondsLeft;
    private readonly int _type;
    private int _spinStep;
    private double _timeSinceLastFrameUpdate;
    private readonly bool _xFlip;
    
    public PipeBombProjectile(Level level, Position position, double directionRadians)
        : base(level, position, Cfg.PipeBombBoundingBox, _damage, directionRadians)
    {
        _lifetimeSecondsLeft = _maxLifetime;
        _type = 1;
        _spinStep = 0;
        _xFlip = (Math.PI / 2) < directionRadians && directionRadians < (3 * Math.PI / 2);

        _xVelocity = Math.Cos(_directionRadians);
        _yVelocity = Math.Sin(_directionRadians);
        _zVelocity = _startingZVelocity;
    }

    public override void Update(double elapsedTime)
    {
        _lifetimeSecondsLeft -= elapsedTime;
        if (_lifetimeSecondsLeft <= 0)
        {
            _level.Explode(this);
            Removed = true;
            return;
        }

        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    private void _UpdatePosition(double elapsedTime)
    {
        double elapsedTimeConstant = 60 * elapsedTime;
        if (IsOnGround)
        {
            _xVelocity *= Math.Pow(_groundFriction, elapsedTimeConstant);
            _yVelocity *= Math.Pow(_groundFriction, elapsedTimeConstant);
        }
        else
        {
            _xVelocity *= Math.Pow(_drag, elapsedTimeConstant);
            _yVelocity *= Math.Pow(_drag, elapsedTimeConstant);
        }
        _zVelocity -= _gravity * elapsedTime;
        _AttemptMove();
    }
    
    private void _UpdateSprite(double elapsedTime)
    {
        _timeSinceLastFrameUpdate += elapsedTime;
        
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            if (IsOnGround)
            {
                _spinStep = (_spinStep & 1) == 0 ?
                    _spinStep + 1 : 
                    _spinStep;
                return;
            }
            _spinStep = (_spinStep + 1) % 4;
            _timeSinceLastFrameUpdate -= _frameDuration;
        }
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = Art.Projectiles[_type][_spinStep];
        screen.Draw(bitmap, xCorrected, yCorrected, _xFlip);
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(
            xCorrected + Art.PickupSize - 6,
            yCorrected - Art.PickupSize - 1,
            xCorrected + Art.PickupSize - 2,
            yCorrected - Art.PickupSize - 1,
            Art.ShadowColor,
            Art.ShadowBlend            
        );
    }

    protected override void _Collide(Entity? entity)
    {
        if (entity != null)
        {
            _xVelocity *= _bounce * -1;
            _yVelocity *= _bounce * -1;
        }
        _zVelocity *= _bounce * -1;
    }
}