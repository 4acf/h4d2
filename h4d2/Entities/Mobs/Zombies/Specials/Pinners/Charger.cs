using H4D2.Entities.Mobs.Survivors;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials.Pinners;

public class Charger : Pinner
{
    private const int _chargingFramesOffset = 9;
    private const int _slamFramesOffset = 24;
    private const double _chargeCooldown = 10.0;
    private const double _chargeRange = 50.0;
    private const double _maxChargeTime = 3.0;
    private const double _defaultSpeed = 250;
    private const double _chargeSpeed = 500;
    private const double _slamDelay = 1.5;
    private const double _gravity = 0.5;
    private const double _stumbleHeight = 3.0;
    
    public bool IsCharging { get; private set; }
    public bool IsStumbling { get; private set; }
    public bool IsSlamming { get; private set; }
    public int SlamStep { get; private set; }
    
    private readonly CountdownTimer _chargeTimer;
    private readonly CountdownTimer _chargeCooldownTimer;
    private readonly CountdownTimer _slamTimer;
    
    public Charger(Level level, Position position) 
        : base(level, position, SpecialConfigs.Charger)
    {
        IsCharging = false;
        IsStumbling = false;
        IsSlamming = false;
        SlamStep = -1;
        
        _chargeTimer = new CountdownTimer(_maxChargeTime);
        _chargeCooldownTimer = new CountdownTimer(_chargeCooldown);
        _chargeCooldownTimer.Update(_chargeCooldown);
        _slamTimer = new CountdownTimer(_slamDelay);
    }

    protected override void _StopPinning()
    {
        base._StopPinning();
        IsSlamming = false;
        _collisionExcludedEntity = null;
        _chargeCooldownTimer.Reset();
        SlamStep = -1;
    }
    
    protected override void _UpdateAttackState(double elapsedTime)
    {
        if (IsCharging)
        {
            _UpdateChargeState(elapsedTime);
            return;
        }

        if (IsStumbling)
        {
            _UpdateStumbleState();
            return;
        }
        
        if (IsSlamming)
        {
            _UpdateSlamState(elapsedTime);
            return;
        }

        _chargeCooldownTimer.Update(elapsedTime);
        
        if (_target == null || _target.Removed || _target is not Survivor survivor)
            return;
        
        ReadonlyPosition targetPosition = survivor.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        if (distance > _chargeRange || 
            !survivor.IsOnGround ||
            survivor.IsPinned ||
            !_HasLineOfSight(survivor)
        )
            return;
        
        _directionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        
        if (_chargeCooldownTimer.IsFinished)
        {
            _Charge();
            _chargeCooldownTimer.Reset();
        }
    }

    private void _UpdateChargeState(double elapsedTime)
    {
        _chargeTimer.Update(elapsedTime);
        if (_chargeTimer.IsFinished)
        {
            _StopCharging(_pinTarget);
            _chargeTimer.Reset();
        }
    }

    private void _UpdateStumbleState()
    {
        if (IsOnGround)
        {
            IsStumbling = false;
            _directionRadians = MathHelpers.NormalizeRadians(_directionRadians + Math.PI);
        }
    }
    
    private void _UpdateSlamState(double elapsedTime)
    {
        if (_pinTarget == null || _pinTarget.Removed)
        {
            _StopPinning();
            return;   
        }
        
        _slamTimer.Update(elapsedTime);
        if (_slamTimer.IsFinished)
        {
            _pinTarget.HitBy(this);
            _slamTimer.Reset();
        }
    }
    
    protected override void _UpdatePosition(double elapsedTime)
    {
        if (IsStumbling)
        {
            _UpdateStumblingPosition(elapsedTime);
            return;
        }
        
        if (IsSlamming)
        {
            _velocity.Stop();
            return;
        }
        
        _velocity.X *= 0.5;
        _velocity.Y *= 0.5;

        if (!IsCharging)
        {
            double targetDirection = _target == null ? 
                _directionRadians : 
                Math.Atan2(_target.CenterMass.Y - CenterMass.Y, _target.CenterMass.X - CenterMass.X);
            double directionDiff = targetDirection - _directionRadians;
            directionDiff = Math.Atan2(Math.Sin(directionDiff), Math.Cos(directionDiff));
            _directionRadians += directionDiff * (elapsedTime * _turnSpeed);
            _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        }
        
        double moveSpeed = (_speed * _speedFactor) * elapsedTime;
        _velocity.X += Math.Cos(_directionRadians) * moveSpeed;
        _velocity.Y += Math.Sin(_directionRadians) * moveSpeed;

        _AttemptMove();
    }

    private void _UpdateStumblingPosition(double elapsedTime)
    {
        _velocity.X *= 0.5;
        _velocity.Y *= 0.5;
        double stumbleSpeed = (_speed * _speedFactor) * elapsedTime;
        _velocity.X += Math.Cos(_directionRadians) * stumbleSpeed;
        _velocity.Y += Math.Sin(_directionRadians) * stumbleSpeed;
        _velocity.Z -= _gravity * elapsedTime;
        _AttemptMove();
    }
    
    protected override void _UpdateSprite(double elapsedTime)
    {
        if (IsCharging || IsStumbling)
            _UpdateChargeSprite(elapsedTime);
        else if (IsSlamming)
            _UpdateSlamSprite(elapsedTime);
        else
            base._UpdateSprite(elapsedTime);
    }

    private void _UpdateChargeSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);

        double directionRadians = IsStumbling ?
            MathHelpers.NormalizeRadians(_directionRadians + Math.PI) :
            _directionRadians;
        SpriteDirection spriteDirection = Direction.Intercardinal(directionRadians);
        _xFlip = spriteDirection.XFlip;
        
        while (_frameUpdateTimer.IsFinished)
        {
            _walkStep = (_walkStep + 1) % 4;
            _frame = _walkStep switch
            {
                0 or 2 => 0 + _chargingFramesOffset + (3 * spriteDirection.Offset),
                1 => 1 + _chargingFramesOffset +  (3 * spriteDirection.Offset),
                3 => 2 + _chargingFramesOffset +  (3 * spriteDirection.Offset),
                _ => _frame
            };
            _frameUpdateTimer.AddDuration();
        }
    }

    private void _UpdateSlamSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
        
        SpriteDirection spriteDirection = Direction.Intercardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;

        while (_frameUpdateTimer.IsFinished)
        {
            SlamStep = (SlamStep + 1) % 12;
            int nextFrame = 0;
            nextFrame = SlamStep switch
            {
                <= 4 or 11 => 2,
                5 => 1,
                _ => 0
            };
            _frame = nextFrame + _slamFramesOffset + (spriteDirection.Offset * 3);
            _frameUpdateTimer.AddDuration();
        }
    }

    private void _Charge()
    {
        IsCharging = true;
        _speed = _chargeSpeed;
    }

    private void _StopCharging(Survivor? pinTarget = null)
    {
        IsCharging = false;
        IsSlamming = pinTarget != null;
        _speed = _defaultSpeed;
        _chargeTimer.Reset();
    }

    private void _Stumble()
    {
        IsStumbling = true;
        _directionRadians += Math.PI;
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        _position.Z = _stumbleHeight;
    }
    
    protected override void _Collide(Entity? entity)
    {
        if (!IsCharging)
        {
            base._Collide(entity);
            return;
        }
        
        if (_pinTarget == null && entity is Survivor survivor)
        {
            _Pin(survivor);
            return;
        }

        if (_pinTarget != null && entity is Survivor survivor2)
        {
            // this will stack in certain conditions (e.g. survivor2 is against a wall)
            // i could easily add a cooldown but i'd rather not because its a rare and
            // funny occurrence when a survivor gets crushed and dies instantly
            survivor2.KnockbackHitBy(this);
        }
    }

    protected override void _CollideWall()
    {
        if (IsCharging)
        {
            _pinTarget?.HitBy(this);
            _StopCharging(_pinTarget);
            _Stumble();
        }
    }

    private void _Pin(Survivor survivor)
    {
        _pinTarget = survivor;
        _collisionExcludedEntity = survivor;
        survivor.Pinned(this);
    }
}