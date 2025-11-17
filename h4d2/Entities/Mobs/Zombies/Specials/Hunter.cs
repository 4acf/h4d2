using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Hunter : Special
{
    private const int _crouchingFramesOffset = 9;
    private const int _jumpingFramesOffset = 12;
    private const int _pinningFramesOffset = 15;
    private const double _crouchTime = 0.75;
    private const double _gravity = 4.0;
    private const double _crouchRange = 50.0;
    private const double _jumpRange = 75.0;
    private const double _attackDelay = 0.5;
    private const double _jumpSpeedScale = 2.0;
    private const double _jumpZVelocity = 1.5;
    private const double _survivorHeight = 2;

    private int _attackStep;
    private bool _isCrouching;
    private bool _isJumping;
    private bool _isPinning;
    private readonly CountdownTimer _crouchTimer;
    private readonly CountdownTimer _attackTimer;
    private Survivor? _pinTarget;
    
    public Hunter(Level level, Position position) 
        : base(level, position, SpecialConfigs.Hunter)
    {
        _attackStep = 0;
        _isCrouching = false;
        _isJumping = false;
        _isPinning = false;
        _crouchTimer = new CountdownTimer(_crouchTime);
        _attackTimer = new CountdownTimer(_attackDelay);
        _pinTarget = null;
    }
    
    protected override void _UpdateAttackState(double elapsedTime)
    {
        if (_isPinning)
        {
            _UpdatePinState(elapsedTime);
            return;
        }

        if (!_isJumping && !_isCrouching)
        {
            _UpdateChaseState();
            return;
        }
        
        _UpdateCrouchState(elapsedTime);
    }

    private void _UpdateCrouchState(double elapsedTime)
    {
        if (IsOnGround)
            _isJumping = false;

        if (_isJumping)
            return;
        
        _crouchTimer.Update(elapsedTime);
        
        if (_target == null || _target.Removed)
            return;
        
        ReadonlyPosition targetPosition = _target.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        _directionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        if (distance > _jumpRange)
            return;
        
        if (_crouchTimer.IsFinished)
        {
            _Jump();
            _crouchTimer.Reset();
        }
    }
    
    private void _UpdateChaseState()
    {
        if (_target == null || _target.Removed)
            return;
        
        ReadonlyPosition targetPosition = _target.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);

        if (distance > _crouchRange)
            return;

        _isCrouching = true;
    }
    
    private void _UpdatePinState(double elapsedTime)
    {
        if (_pinTarget == null || _pinTarget.Removed)
        {
            _isPinning = false;
            _pinTarget = null;
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
        if(_isJumping)
        {
            _velocity.Z -= _gravity * elapsedTime;
            _AttemptMove();
        }
        else if (_isCrouching || _isPinning)
        {
            _velocity.Stop();
        }
        else
        {
            base._UpdatePosition(elapsedTime);
        }
    }
    
    protected override void _UpdateSprite(double elapsedTime)
    {
        if (_isCrouching)
            _UpdateUnchangingSprite(_crouchingFramesOffset);
        else if (_isJumping)
            _UpdateUnchangingSprite(_jumpingFramesOffset);
        else if (_isPinning)
            _UpdatePinningSprite(elapsedTime);
        else
            base._UpdateSprite(elapsedTime);
    }

    private void _UpdateUnchangingSprite(int offset)
    {
        SpriteDirection spriteDirection = Direction.Cardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;
        _frame = offset + spriteDirection.Offset;
    }

    private void _UpdatePinningSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
        
        SpriteDirection spriteDirection = Direction.Cardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;

        while (_frameUpdateTimer.IsFinished)
        {
            _attackStep = _attackStep == 0 ? 1 : 0;
            _frame = _pinningFramesOffset + _attackStep + (spriteDirection.Offset * 2);
            _frameUpdateTimer.AddDuration();
        }
    }
    
/*  FOR NOW, UNCOMMENT THIS WHEN CROUCHING AND POUNCING IS FIGURED OUT AND U HAVE THE BITMAPS
    protected override void _Collide(Entity? entity)
    {
        base._Collide(entity);
        if (!_isJumping || entity is not Survivor survivor)
            return;
        if (survivor.IsPinned)
            return;
        _Pin(survivor);
    }
*/
    
    private void _Jump()
    {
        _isCrouching = false;
        _isJumping = true;
        _velocity.X = Math.Cos(_directionRadians) * _jumpSpeedScale;
        _velocity.Y = Math.Sin(_directionRadians) * _jumpSpeedScale;
        _velocity.Z = _jumpZVelocity;
    }
    
    private void _Pin(Survivor survivor)
    {
        _isJumping = false;
        _isPinning = true;
        _pinTarget = survivor;
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