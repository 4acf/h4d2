using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles.ThrowableProjectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;

public class Uncommon : Zombie
{
    protected const double _attackRange = 5.0;
    protected const double _attackDelay = 1.0;
    protected const double _pipeBombIdleDistance = 7.5;
    protected const double _bileBombRageDistance = 10.0;
    
    protected readonly int _type;
    protected double _aimDirectionRadians;
    protected readonly CountdownTimer _attackDelayTimer;
    protected BileBombProjectile? _bileBombTarget;
    
    protected Uncommon(Level level, Position position, UncommonConfig config) 
        : base(level, position, config)
    {
        _type = config.Type;
        _attackDelayTimer = new CountdownTimer(_attackDelay);
        _bileBombTarget = null;
    }
    
    public override void Update(double elapsedTime)
    {
        _hazardDamageTimer.Update(elapsedTime);
        _UpdateAttackState(elapsedTime);
        _UpdateTarget();
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    private void _UpdateAttackState(double elapsedTime)
    {
        _attackDelayTimer.Update(elapsedTime);
        if (_target == null || _target.Removed)
        {
            _isAttacking = false;
            return;
        }

        if (_target is not Mob targetMob)
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
            targetMob.HitBy(this);
            _attackDelayTimer.Reset();
        }
                
        _aimDirectionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
        _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
    }
    
    protected virtual void _UpdateTarget()
    {
        if (_bileBombTarget != null)
        {
            if (_bileBombTarget.Removed)
            {
                _target = null;
                _bileBombTarget = null;
            }
            else
            {
                ReadonlyPosition bileBombPosition = _bileBombTarget.CenterMass;
                ReadonlyPosition zombiePosition = FootPosition;
                double distance = ReadonlyPosition.Distance(bileBombPosition, zombiePosition);
                if (distance < _bileBombRageDistance)
                {
                    _target = _level.GetNearestEntity<Zombie>(Position, this);
                }
                else
                {
                    // this is here in the event the rage target dies 
                    // and the zombie had chased it outside the range of the bile
                    _target = _bileBombTarget; 
                }
            }
            return;
        }

        BileBombProjectile? activeBileBomb = _level.GetNearestEntity<BileBombProjectile>(Position);
        if (activeBileBomb != null)
        {
            _target = activeBileBomb;
            _bileBombTarget = activeBileBomb;
            return;
        }
        
        PipeBombProjectile? activePipeBomb = _level.GetNearestEntity<PipeBombProjectile>(Position);
        if (activePipeBomb != null)
        {
            _target = activePipeBomb;
            return;
        }
        
        _target = _level.GetNearestEntity<Survivor>(Position);
    }
    
    private void _UpdatePosition(double elapsedTime)
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

        if (_target is PipeBombProjectile)
        {
            ReadonlyPosition targetPosition = _target.CenterMass;
            ReadonlyPosition zombiePosition = FootPosition;
            double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
            if (distance < _pipeBombIdleDistance)
            {
                _velocity.X = 0;
                _velocity.Y = 0;
            }
        }
        
        _AttemptMove();
    }
    
    private void _UpdateSprite(double elapsedTime)
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
        double degrees = MathHelpers.RadiansToDegrees(_aimDirectionRadians);
        switch (degrees)
        {
            case >= 337.5:
            case < 22.5:
                direction = 2;
                _xFlip = false;
                break;
            case < 67.5:
                direction = 3;
                _xFlip = false;
                break;
            case < 112.5:
                direction = 4;
                _xFlip = false;
                break;
            case < 157.5:
                direction = 3;
                _xFlip = true;
                break;
            case < 202.5:
                direction = 2;
                _xFlip = true;
                break;
            case < 247.5:
                direction = 1;
                _xFlip = true;
                break;
            case < 292.5:
                direction = 0;
                _xFlip = false;
                break;
            default:
                direction = 1;
                _xFlip = false;
                break;
        }
        
        while (_frameUpdateTimer.IsFinished)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_velocity.X == 0 && _velocity.Y == 0) _walkStep = 0;
            int nextLowerFrame = 0;
            if (direction == 2)
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
            _upperFrame = _attackingBitmapOffset + direction;
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
        Bitmap lowerBitmap = Art.Uncommons[_type][_lowerFrame];
        Bitmap upperBitmap = Art.Uncommons[_type][_upperFrame];
        screen.Draw(lowerBitmap, xCorrected, yCorrected, _xFlip);
        screen.Draw(upperBitmap, xCorrected, yCorrected, _xFlip);
    }
    
    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(
            xCorrected + Art.SpriteSize - 10,
            yCorrected - Art.SpriteSize - 1,
            xCorrected + Art.SpriteSize - 7,
            yCorrected - Art.SpriteSize - 1
        );
    }
}