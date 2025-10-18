using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Commons;
using Cfg = CommonConfig;

public class Common : Mob
{
    private readonly int _common;
    private int _walkStep;
    private int _walkFrame;
    private const double _frameDuration = 1.0 / 8.0;
    private double _timeSinceLastFrameUpdate;
    private Entity? _target;
    
    public Common(Level level, int xPosition, int yPosition)
        : base(
            level,
            Cfg.BoundingBox, 
            Cfg.Health, 
            RandomSingleton.Instance.Next(Cfg.MinSpeed, Cfg.MaxSpeed), 
            xPosition, 
            yPosition
            )
    {
        _common = RandomSingleton.Instance.Next(Cfg.NumSprites);
        _walkStep = 0;
        _walkFrame = 0;
        _timeSinceLastFrameUpdate = 0.0;
        _target = null;
    }

    public override void Update(double elapsedTime)
    {
        _UpdateTarget();
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    private void _UpdateTarget()
    {
        if (_target != null) return;
        _target = _level.GetNearestHealthySurvivor(XPosition, YPosition);
    }
    
    private void _UpdatePosition(double elapsedTime)
    {
        _xVelocity *= 0.5;
        _yVelocity *= 0.5;
        
        double targetDirection = _target == null ? 
            _directionRadians : 
            Math.Atan2(_target.YPosition - YPosition, _target.XPosition - XPosition);
        double directionDiff = targetDirection - _directionRadians;
        directionDiff = Math.Atan2(Math.Sin(directionDiff), Math.Cos(directionDiff));
        _directionRadians += directionDiff * (elapsedTime * _turnSpeed);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        
        double moveSpeed = (0.2 * _speed / 220) * 50 * elapsedTime;
        _xVelocity += Math.Cos(_directionRadians) * moveSpeed;
        _yVelocity += Math.Sin(_directionRadians) * moveSpeed;

        _AttemptMove();
    }
    
    private void _UpdateSprite(double elapsedTime)
    {
        _timeSinceLastFrameUpdate += elapsedTime;

        int direction = 0;
        int degrees = MathHelpers.RadiansToDegrees(_directionRadians);
        switch (degrees)
        {
            case >= 315:
            case < 45:
                direction = 1;
                _xFlip = false;
                break;
            case < 135:
                direction = 2;
                _xFlip = false;
                break;
            case < 225:
                direction = 1;
                _xFlip = true;
                break;
            default:
                direction = 0;
                _xFlip = false;
                break;
        }
        
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_xVelocity == 0 && _yVelocity == 0) _walkStep = 0;
            int nextFrame = 0;
            if (direction == 1)
            {
                nextFrame = _walkStep switch
                {
                    0 => 3,
                    1 or 3 => 4,
                    2 => 5,
                    _ => nextFrame
                };
            }
            else
            {
                nextFrame = _walkStep switch
                {
                    0 or 2 => 0 + (3 * direction),
                    1 => 1 + (3 * direction),
                    3 => 2 + (3 * direction),
                    _ => nextFrame
                };
            }
            _walkFrame = nextFrame;
            _timeSinceLastFrameUpdate -= _frameDuration;
        }
    }

    public override void Render(Bitmap screen)
    {
        Bitmap animationCycleBitmap = Art.Commons[_common][_walkFrame];
        screen.Draw(animationCycleBitmap, (int)XPosition, (int)YPosition, _xFlip);
    }
    
    public override void RenderShadow(Bitmap screen)
    {
        int x = (int)XPosition;
        int y = (int)YPosition;
        screen.BlendFill(
            x + Art.SpriteSize - 10,
            y - Art.SpriteSize - 1,
            x + Art.SpriteSize - 7,
            y - Art.SpriteSize - 1,
            0x0,
            0.9            
        );
    }
}