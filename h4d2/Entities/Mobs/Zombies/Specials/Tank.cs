using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Tank : Special
{
    private const double _attackRange = 15.0;
    private const double _attackDelay = 0.5;
    private const double _attackCompleteTime = 0.5;
 
    public double AimDirectionRadians { get; private set; }
    
    private int _attackFrame;
    private readonly CountdownTimer _attackDelayTimer;
    private readonly CountdownTimer _attackCompleteTimer;
    
    public Tank(Level level, Position position) 
        : base(level, position, SpecialConfigs.Tank)
    {
        _attackFrame = -1;
        AimDirectionRadians = 0.0;
        _attackDelayTimer = new CountdownTimer(_attackDelay);
        _attackDelayTimer.Update(_attackDelay);
        _attackCompleteTimer = new CountdownTimer(_attackCompleteTime);
    }
    
    protected override void _UpdateAttackState(double elapsedTime)
    {
        if (_isAttacking)
        {
            _attackCompleteTimer.Update(elapsedTime);
            if (_attackCompleteTimer.IsFinished)
            {
                _isAttacking = false;
            }
            return;
        }
        
        _attackDelayTimer.Update(elapsedTime);
        if (_target == null || _target.Removed)
        {
            _isAttacking = false;
            return;
        }

        if (_target is not Survivor survivor)
        {
            _isAttacking = false;
            return;
        }
        
        ReadonlyPosition targetPosition = _target.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        _isAttacking = distance <= _attackRange;
        if (!_isAttacking) 
            return;
        
        if (_attackDelayTimer.IsFinished)
        {
            _attackFrame = -1;
            survivor.HitBy(this);
            _attackDelayTimer.Reset();
            _attackCompleteTimer.Reset();
        }
                
        AimDirectionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
        AimDirectionRadians = MathHelpers.NormalizeRadians(AimDirectionRadians);
    }

    protected override void _UpdateSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
        if (_isAttacking)
            _UpdateAttackingSprite();
        else
            _UpdateRunningSprite();
    }

    private void _UpdateAttackingSprite()
    {
        int direction = 0;
        double degrees = MathHelpers.RadiansToDegrees(AimDirectionRadians);
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
            if(_attackFrame != 2)
                _attackFrame = (_attackFrame + 1) % 3;
            if (_velocity.X == 0 && _velocity.Y == 0) _walkStep = 0;
            int nextLowerFrame = 0;
            if (direction == 1)
            {
                nextLowerFrame = _walkStep switch
                {
                    0 => 3,
                    1 or 3 => 4,
                    2 => 5,
                    _ => nextLowerFrame
                };
            }
            else
            {
                nextLowerFrame = _walkStep switch
                {
                    0 or 2 => 0,
                    1 => 1,
                    3 => 2,
                    _ => nextLowerFrame
                };
            }
            _lowerFrame = nextLowerFrame;
            _upperFrame = _attackingBitmapOffset + (direction * 3) + _attackFrame;
            _frameUpdateTimer.AddDuration();
        }
    }

    private void _UpdateRunningSprite()
    {
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

            _lowerFrame = nextFrame;
            _upperFrame = nextFrame + _upperBitmapOffset;
            _frameUpdateTimer.AddDuration();
        }
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap lowerBitmap = H4D2Art.Specials[_type][_lowerFrame];
        Bitmap upperBitmap = H4D2Art.Specials[_type][_upperFrame];
        screen.Draw(lowerBitmap, xCorrected, yCorrected, _xFlip);
        screen.Draw(upperBitmap, xCorrected, yCorrected, _xFlip);
    }
}