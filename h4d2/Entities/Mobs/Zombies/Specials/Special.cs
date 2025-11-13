using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public abstract class Special : Zombie
{
    protected int _frame;
    private readonly int _type;
    
    protected Special(Level level, Position position, SpecialConfig config) 
        : base(level, position, config)
    {
        _type = config.Type;
    }

    public override void Update(double elapsedTime)
    {
        _hazardDamageTimer.Update(elapsedTime);
        _UpdateAttackState(elapsedTime);
        _UpdateTarget();
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    protected virtual void _UpdateAttackState(double elapsedTime)
    {
        
    }
    
    protected virtual void _UpdateTarget()
    {
        _target = _level.GetNearestEntity<Survivor>(Position);
    }
    
    protected virtual void _UpdatePosition(double elapsedTime)
    {
        _velocity.X *= 0.5;
        _velocity.Y *= 0.5;
        
        double targetDirection = _target == null ? 
            _directionRadians : 
            Math.Atan2(_target.CenterMass.Y - CenterMass.Y, _target.CenterMass.X - CenterMass.X);
        double directionDiff = targetDirection - _directionRadians;
        directionDiff = Math.Atan2(Math.Sin(directionDiff), Math.Cos(directionDiff));
        _directionRadians += directionDiff * (elapsedTime * _turnSpeed);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        
        double moveSpeed = (_speed * _speedFactor) * elapsedTime;
        _velocity.X += Math.Cos(_directionRadians) * moveSpeed;
        _velocity.Y += Math.Sin(_directionRadians) * moveSpeed;

        _AttemptMove();
    }
    
    protected virtual void _UpdateSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);

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
        
        while (_frameUpdateTimer.IsFinished)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_velocity.X == 0 && _velocity.Y == 0) _walkStep = 0;
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
            _frame = nextFrame;
            _frameUpdateTimer.AddDuration();
        }
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap animationCycleBitmap = H4D2Art.Specials[_type][_frame];
        screen.Draw(animationCycleBitmap, xCorrected, yCorrected, _xFlip);
    }
    
    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + H4D2Art.SpriteSize - 10,
            yCorrected - H4D2Art.SpriteSize - 1,
            xCorrected + H4D2Art.SpriteSize - 7,
            yCorrected - H4D2Art.SpriteSize - 1
        );
    }
}