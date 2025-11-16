using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Jockey : Special
{
    public double DirectionRadians => _directionRadians;
    
    private const int _jumpingFramesOffset = 9;
    private const int _pinningFramesOffset = 12;
    private const double _jumpCooldown = 0.5;
    private const double _gravity = 4.0;
    private const double _jumpRange = 30.0;
    private const double _attackDelay = 0.5;
    private const double _jumpSpeedScale = 2.0;
    private const double _jumpZVelocity = 1.0;
    private const int _boundaryTolerance = 25;
    private const int _survivorHeight = 9;
    
    private bool _isJumping;
    private bool _isPinning;
    private double _aimDirectionRadians;
    private readonly CountdownTimer _jumpTimer;
    private readonly CountdownTimer _attackTimer;
    private Survivor? _pinTarget; 
    
    public Jockey(Level level, Position position) 
        : base(level, position, SpecialConfigs.Jockey)
    {
        _isJumping = false;
        _isPinning = false;
        _aimDirectionRadians = 0.0;
        _jumpTimer = new CountdownTimer(_jumpCooldown);
        _attackTimer = new CountdownTimer(_attackDelay);
        _pinTarget = null;
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
        
        _jumpTimer.Update(elapsedTime);
        
        if (_target == null || _target.Removed)
            return;
        
        ReadonlyPosition targetPosition = _target.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        if (distance > _jumpRange)
            return;
        
        if (_jumpTimer.IsFinished)
        {
            _aimDirectionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
            _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
            _Jump();
            _jumpTimer.Reset();
        }
    }

    private void _UpdatePinState(double elapsedTime)
    {
        if (_pinTarget == null || _pinTarget.Removed)
        {
            _isPinning = false;
            _pinTarget = null;
            _collisionExcludedEntity = null;
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
        direction = _CorrectDirectionToAvoidWalls(direction);
        return direction;
    }

    private double _CorrectDirectionToAvoidWalls(double direction)
    {
        ReadonlyPosition centerMass = CenterMass;
        
        if (centerMass.X < _boundaryTolerance)
        {
            if ((Math.PI / 2) < direction && direction < (3 * Math.PI / 2))
            {
                direction = Math.Atan2(Math.Sin(direction), Math.Cos(direction) * -1);
            }
        }
        
        if (centerMass.Y < _boundaryTolerance)
        {
            if (direction > Math.PI)
            {
                direction = Math.Atan2(Math.Sin(direction) * -1, Math.Cos(direction));
            }
        }

        if (_level.Width - centerMass.X < _boundaryTolerance)
        {
            if ((3 * Math.PI / 2) < direction || direction < (Math.PI / 2))
            {
                direction = Math.Atan2(Math.Sin(direction), Math.Cos(direction) * -1);
            }
        }

        if (_level.Height - centerMass.Y < _boundaryTolerance)
        {
            if (direction < Math.PI)
            {
                direction = Math.Atan2(Math.Sin(direction) * -1, Math.Cos(direction));
            }
        }

        return direction;
    }
    
    protected override void _UpdateSprite(double elapsedTime)
    {
        if(_isPinning)
            _UpdateNonRunningSprite(_pinningFramesOffset);
        else if(_isJumping)
            _UpdateNonRunningSprite(_jumpingFramesOffset);
        else
            base._UpdateSprite(elapsedTime);
    }

    private void _UpdateNonRunningSprite(int offset)
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
        
        _frame = offset + direction;
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
        if (!_isJumping || entity is not Survivor survivor)
            return;
        if (survivor.IsPinned)
            return;
        _isPinning = true;
        _isJumping = false;
        _pinTarget = survivor;
        _collisionExcludedEntity = survivor;
        survivor.Pinned(this);
        _position.X = survivor.Position.X;
        _position.Y = survivor.Position.Y;
        _position.Z = _survivorHeight;
    }
    
    protected override void _Die()
    {
        base._Die();
        _pinTarget?.Cleared();
    }
}