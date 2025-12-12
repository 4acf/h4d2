using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials.Pinners;

public class Jockey : Pinner
{
    private const int _jumpingFramesOffset = 9;
    private const int _pinningFramesOffset = 12;
    private const double _jumpCooldown = 0.5;
    private const double _rejockCooldown = 6.0;
    private const double _gravity = 4.0;
    private const double _jumpRange = 30.0;
    private const double _attackDelay = 0.5;
    private const double _jumpSpeedScale = 2.0;
    private const double _jumpZVelocity = 1.0;
    
    private bool _isJumping;
    private bool _isPinning;
    private readonly CountdownTimer _jumpTimer;
    private readonly CountdownTimer _rejockTimer;
    private readonly CountdownTimer _attackTimer;
    
    public Jockey(Level level, Position position) 
        : base(level, position, SpecialConfigs.Jockey)
    {
        _isJumping = false;
        _isPinning = false;
        _jumpTimer = new CountdownTimer(_jumpCooldown);
        _rejockTimer = new CountdownTimer(_rejockCooldown);
        _rejockTimer.Update(_rejockCooldown);
        _attackTimer = new CountdownTimer(_attackDelay);
    }

    protected override void _StopPinning()
    {
        base._StopPinning();
        _isPinning = false;
        _collisionExcludedEntity = null;
        _rejockTimer.Reset();
    }

    protected override void _UpdateAttackState(double elapsedTime)
    {
        if(!_isPinning)
            _UpdateJumpState(elapsedTime);
        else
            _UpdatePinState(elapsedTime);
    }

    private void _UpdateJumpState(double elapsedTime)
    {
        if (IsOnGround)
            _isJumping = false;
        
        if (_isJumping)
            return;
        
        _rejockTimer.Update(elapsedTime);
        if (!_rejockTimer.IsFinished)
            return;
        
        _jumpTimer.Update(elapsedTime);
        
        if (_target == null || _target.Removed)
            return;
        
        ReadonlyPosition targetPosition = _target.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        if (distance > _jumpRange || !_HasLineOfSight(_target))
            return;
        
        if (_jumpTimer.IsFinished)
        {
            _directionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
            _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
            _Jump();
            _jumpTimer.Reset();
        }
    }

    private void _UpdatePinState(double elapsedTime)
    {
        if (_pinTarget == null || _pinTarget.Removed)
        {
            _StopPinning();
            return;
        }
        
        _attackTimer.Update(elapsedTime);
        if (_attackTimer.IsFinished)
        {
            _pinTarget.HitBy(this);
            _attackTimer.Reset();
        }
    }
    
    protected override void _UpdatePosition(double elapsedTime)
    {
        if (_isPinning)
        {
            _UpdatePositionRandomly(elapsedTime);
            return;
        }
        
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

    private void _UpdatePositionRandomly(double elapsedTime)
    {
        _velocity.X *= 0.5;
        _velocity.Y *= 0.5;

        double targetDirection = _CalculateBestDirection();
        double directionDiff = targetDirection - _directionRadians;
        directionDiff = Math.Atan2(Math.Sin(directionDiff), Math.Cos(directionDiff));
        _directionRadians += directionDiff * (elapsedTime * _turnSpeed);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
    
        double moveSpeed = (_speed * _speedFactor) * elapsedTime;
        _velocity.X += Math.Cos(_directionRadians) * moveSpeed;
        _velocity.Y += Math.Sin(_directionRadians) * moveSpeed;
        _AttemptMove();
    }
    
    private double _CalculateBestDirection()
    {
        double direction = RandomSingleton.Instance.NextDouble() * (2 * Math.PI);
        return direction;
    }
    
    protected override void _UpdateSprite(double elapsedTime)
    {
        if(_isPinning)
            _UpdateNonRunningSprite(elapsedTime, _pinningFramesOffset);
        else if(_isJumping)
            _UpdateNonRunningSprite(elapsedTime, _jumpingFramesOffset);
        else
            base._UpdateSprite(elapsedTime);
    }

    private void _UpdateNonRunningSprite(double elapsedTime, int offset)
    {
        _frameUpdateTimer.Update(elapsedTime);
        while (_frameUpdateTimer.IsFinished)
        {
            SpriteDirection spriteDirection = Direction.Cardinal(_directionRadians);
            _xFlip = spriteDirection.XFlip;
            _frame = offset + spriteDirection.Offset;
            _frameUpdateTimer.AddDuration();
        }
    }
    
    private void _Jump()
    {
        _isJumping = true;
        _velocity.X = Math.Cos(_directionRadians) * _jumpSpeedScale;
        _velocity.Y = Math.Sin(_directionRadians) * _jumpSpeedScale;
        _velocity.Z = _jumpZVelocity;
    }
    
    protected override void _Collide(Entity? entity)
    {
        base._Collide(entity);
        if (!_isJumping || entity is not Survivor survivor)
            return;
        if (survivor.IsPinned)
            return;
        _Pin(survivor);
    }

    private void _Pin(Survivor survivor)
    {
        _isPinning = true;
        _isJumping = false;
        _pinTarget = survivor;
        _collisionExcludedEntity = survivor;
        survivor.Pinned(this);
        _position.X = survivor.Position.X;
        _position.Y = survivor.Position.Y;
        _position.Z = 0;
    }
}