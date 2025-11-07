using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

public class PipeBombProjectile : ThrowableProjectile
{
    public const double SplashRadius = 50.0;
    private const double _lifetime = 6.0;
    private const double _bounce = 0.6;
    private const double _groundFriction = 0.6;
    
    private readonly CountdownTimer _explodeTimer;
    
    public PipeBombProjectile(Level level, Position position, double directionRadians)
        : base(level, position, ThrowableProjectileConfigs.PipeBomb, directionRadians)
    {
        _explodeTimer = new CountdownTimer(_lifetime);
    }

    public override void Update(double elapsedTime)
    {
        _explodeTimer.Update(elapsedTime);
        if (_explodeTimer.IsFinished)
        {
            _level.Explode(this);
            Removed = true;
            return;
        }

        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    protected override void _UpdatePosition(double elapsedTime)
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
    
    protected override void _UpdateSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
        while (_frameUpdateTimer.IsFinished)
        {
            if (IsOnGround)
            {
                _spinStep = (_spinStep & 1) == 0 ?
                    _spinStep + 1 : 
                    _spinStep;
                return;
            }
            _spinStep = (_spinStep + 1) % 4;
            _frameUpdateTimer.AddDuration();
        }
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(
            xCorrected + Art.ProjectileSize - 6,
            yCorrected - Art.ProjectileSize - 1,
            xCorrected + Art.ProjectileSize - 2,
            yCorrected - Art.ProjectileSize - 1,
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