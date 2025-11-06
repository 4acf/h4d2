using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Projectiles.ThrowableProjectiles;

public abstract class ThrowableProjectile : Projectile
{
    protected const double _frameDuration = 1.0 / 8.0;
    protected const double _gravity = 2.2;
    protected const double _drag = 0.999;
    protected const double _startingZVelocity = 1.0;
    
    protected readonly int _type;
    protected int _spinStep;
    protected double _timeSinceLastFrameUpdate;
    protected readonly bool _xFlip;
    
    protected ThrowableProjectile
        (Level level, Position position, ThrowableProjectileConfig config, double directionRadians)
        : base(level, position, config.BoundingBox, config.Damage, directionRadians)
    {
        _type = config.Type;
        _spinStep = 0;
        _timeSinceLastFrameUpdate = 0.0;
        _xFlip = (Math.PI / 2) < directionRadians && directionRadians < (3 * Math.PI / 2);
        _xVelocity = Math.Cos(_directionRadians);
        _yVelocity = Math.Sin(_directionRadians);
        _zVelocity = _startingZVelocity;
    }
    
    protected virtual void _UpdatePosition(double elapsedTime)
    {
        double elapsedTimeConstant = 60 * elapsedTime;
        _xVelocity *= Math.Pow(_drag, elapsedTimeConstant);
        _yVelocity *= Math.Pow(_drag, elapsedTimeConstant);
        _zVelocity -= _gravity * elapsedTime;
        _AttemptMove();
    }
    
    protected virtual void _UpdateSprite(double elapsedTime)
    {
        _timeSinceLastFrameUpdate += elapsedTime;
        
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            _spinStep = (_spinStep + 1) % 4;
            _timeSinceLastFrameUpdate -= _frameDuration;
        }
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = Art.Projectiles[_type][_spinStep];
        screen.Draw(bitmap, xCorrected, yCorrected, _xFlip);
    }
}