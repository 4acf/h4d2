using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Jockey : Special
{
    private const int _jumpingFramesOffset = 9;
    private const int _latchedFramesOffset = 12;
    private const double _jumpCooldown = 0.5;
    private const double _gravity = 4.0;
    private const double _attackRange = 30.0;
    private const double _jumpSpeedScale = 2.0;
    private const double _jumpZVelocity = 1.0;
    
    private bool _isJumping;
    private bool _isLatched;
    private double _aimDirectionRadians;
    private readonly CountdownTimer _jumpTimer;
    
    public Jockey(Level level, Position position) 
        : base(level, position, SpecialConfigs.Jockey)
    {
        _isJumping = false;
        _isLatched = false;
        _aimDirectionRadians = 0.0;
        _jumpTimer = new CountdownTimer(_jumpCooldown);
    }

    protected override void _UpdateAttackState(double elapsedTime)
    {
        if(!_isLatched)
            _UpdateJumpState(elapsedTime);
        else
            _UpdateLatchState(elapsedTime);
    }

    private void _UpdateJumpState(double elapsedTime)
    {
        if (IsOnGround)
            _isJumping = false;
        
        if (_isJumping)
            return;
        
        _jumpTimer.Update(elapsedTime);
        
        if (_target == null || _target.Removed)
            return;
        
        ReadonlyPosition targetPosition = _target.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        if (distance > _attackRange)
            return;
        
        if (_jumpTimer.IsFinished)
        {
            _aimDirectionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
            _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
            _Jump();
            _jumpTimer.Reset();
        }
    }

    private void _UpdateLatchState(double elapsedTime)
    {
        
    }
    
    protected override void _UpdatePosition(double elapsedTime)
    {
        if (IsOnGround)
        {
            base._UpdatePosition(elapsedTime);
        }
        else
        {
            _velocity.Z -= _gravity * elapsedTime;
            _AttemptMove();
        }
    }

    protected override void _UpdateSprite(double elapsedTime)
    {
        if(_isJumping)
            _UpdateJumpingSprite();
        else
            base._UpdateSprite(elapsedTime);
    }

    private void _UpdateJumpingSprite()
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
        
        _frame = _jumpingFramesOffset + direction;
    }
    
    private void _Jump()
    {
        _isJumping = true;
        _velocity.X = Math.Cos(_aimDirectionRadians) * _jumpSpeedScale;
        _velocity.Y = Math.Sin(_aimDirectionRadians) * _jumpSpeedScale;
        _velocity.Z = _jumpZVelocity;
    }
    
    protected override void _Collide(Entity? entity)
    {
        base._Collide(entity);
    }
}