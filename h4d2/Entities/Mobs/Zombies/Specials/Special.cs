using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public abstract class Special : Zombie
{
    protected int _frame;
    protected readonly int _type;
    
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

    protected abstract void _UpdateAttackState(double elapsedTime);
    
    protected virtual void _UpdateTarget()
    {
        _target = _level.GetNearestEntity<Survivor>(Position);
    }
    
    protected virtual void _UpdatePosition(double elapsedTime)
    {
        _velocity.X *= 0.5;
        _velocity.Y *= 0.5;
        
        double targetDirection = 0.0;
        if (_target != null && _pathfinder.HasLineOfSight(_target))
        {
            targetDirection = 
                Math.Atan2(_target.CenterMass.Y - CenterMass.Y, _target.CenterMass.X - CenterMass.X);
            _pathfinder.InvalidatePath();
        }
        else
        {
            targetDirection = _target == null ? 
                _directionRadians : 
                _pathfinder.GetNextDirection(CenterMass, _target.CenterMass);
        }
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

        SpriteDirection spriteDirection = Direction.Cardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;
        
        while (_frameUpdateTimer.IsFinished)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_velocity.X == 0 && _velocity.Y == 0) _walkStep = 0;
            int nextFrame = 0;
            if (spriteDirection.Offset == 1)
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
                    0 or 2 => 0 + (3 * spriteDirection.Offset),
                    1 => 1 + (3 * spriteDirection.Offset),
                    3 => 2 + (3 * spriteDirection.Offset),
                    _ => nextFrame
                };
            }
            _frame = nextFrame;
            _frameUpdateTimer.AddDuration();
        }
    }

    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
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