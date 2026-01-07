using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

public abstract class ThrowableProjectile : Projectile
{
    protected const double _frameDuration = 1.0 / 8.0;
    protected const double _gravity = 2.2;
    protected const double _drag = 0.999;
    protected const double _startingZVelocity = 1.0;
    protected const double _bounce = 0.6;
    
    protected readonly int _type;
    protected int _spinStep;
    protected readonly CountdownTimer _frameUpdateTimer;
    protected readonly bool _xFlip;
    
    protected ThrowableProjectile
        (Level level, Position position, ThrowableProjectileConfig config, double directionRadians)
        : base(level, position, config.BoundingBox, config.Damage, directionRadians)
    {
        _type = config.Type;
        _spinStep = 0;
        _frameUpdateTimer = new CountdownTimer(_frameDuration);
        _xFlip = (Math.PI / 2) < directionRadians && directionRadians < (3 * Math.PI / 2);
        
        (double xOffs, double yOffs) = ScreenSpaceToWorldSpace(
            -H4D2Art.ProjectileSize / 2.0,
            H4D2Art.ProjectileSize
        );
        _position.X += xOffs;
        _position.Y += yOffs;
        
        _velocity.X = Math.Cos(DirectionRadians);
        _velocity.Y = Math.Sin(DirectionRadians);
        _velocity.Z = _startingZVelocity;
    }
    
    protected virtual void _UpdatePosition(double elapsedTime)
    {
        double elapsedTimeConstant = 60 * elapsedTime;
        _velocity.X *= Math.Pow(_drag, elapsedTimeConstant);
        _velocity.Y *= Math.Pow(_drag, elapsedTimeConstant);
        _velocity.Z -= _gravity * elapsedTime;
        _AttemptMove();
    }
    
    protected virtual void _UpdateSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
        while (_frameUpdateTimer.IsFinished)
        {
            _spinStep = (_spinStep + 1) % 4;
            _frameUpdateTimer.AddDuration();
        }
    }
    
    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = H4D2Art.Projectiles[_type][_spinStep];
        screen.Draw(bitmap, xCorrected, yCorrected, _xFlip);
    }
}