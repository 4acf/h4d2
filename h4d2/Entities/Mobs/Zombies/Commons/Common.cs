using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles.ThrowableProjectiles;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Commons;

public class Common : Zombie
{
    private const int _numVariations = 9;
    private const int _minSpeed = 230;
    private const int _maxSpeed = 280;
    private const double _attackRange = 8.0;
    private const double _attackDelay = 1.0;
    private const double _pipeBombIdleDistance = 7.5;
    private const double _bileBombRageDistance = 10.0;
    
    private readonly int _type;
    private readonly CountdownTimer _attackDelayTimer; 
    private BileBombProjectile? _bileBombTarget;
    
    public Common(Level level, Position position)
        : base(level, position, CommonConfigs.Common, RandomSingleton.Instance.Next(_minSpeed, _maxSpeed))
    {
        _type = RandomSingleton.Instance.Next(_numVariations);
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
                
        _directionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
    }
    
    private void _UpdateTarget()
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
        
        Survivor? nearestBiledSurvivor = _level.GetNearestBiledSurvivor(Position);
        if (nearestBiledSurvivor != null)
        {
            _target = nearestBiledSurvivor;
            return;
        }
        
        _target = _level.GetNearestEntity<Survivor>(Position);
    }
    
    private void _UpdatePosition(double elapsedTime)
    {
        _velocity.X *= 0.5;
        _velocity.Y *= 0.5;

        Tile tile = Level.GetTilePosition(CenterMass);
        if (_level.IsWall(tile))
        {
            const double halfPi = Math.PI / 2;
            int multiplier = 0;
            
            if (!_level.IsWall(tile.X + 1, tile.Y))
                multiplier = 0;
            else if (!_level.IsWall(tile.X, tile.Y - 1))
                multiplier = 1;
            else if (!_level.IsWall(tile.X - 1, tile.Y))
                multiplier = 2;
            else
                multiplier = 3;
                
            _directionRadians = multiplier * halfPi;
        }
        else
        {
            double targetDirection = _GetTargetDirection();
            double directionDiff = targetDirection - _directionRadians;
            directionDiff = Math.Atan2(Math.Sin(directionDiff), Math.Cos(directionDiff));
            _directionRadians += directionDiff * (elapsedTime * _turnSpeed);
            _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        }
        
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

    private double _GetTargetDirection()
    {
        if (_target != null && _pathfinder.HasLineOfSight(_target))
        {
            _pathfinder.InvalidatePath();
            return Math.Atan2(
                _target.CenterMass.Y - CenterMass.Y,
                _target.CenterMass.X - CenterMass.X
            );
        }
        return _target == null ? 
            _directionRadians : 
            _pathfinder.GetNextDirection(CenterMass, _target.CenterMass);
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
        SpriteDirection spriteDirection = Direction.Intercardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;
        
        while (_frameUpdateTimer.IsFinished)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_velocity.X == 0 && _velocity.Y == 0) _walkStep = 0;
            int nextLowerFrame = 0;
            if (spriteDirection.Offset == 2)
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
            _upperFrame = _attackingBitmapOffset + spriteDirection.Offset;
            _frameUpdateTimer.AddDuration();
        }
    }

    private void _UpdateRunningSprite()
    {
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

            _lowerFrame = nextFrame;
            _upperFrame = nextFrame + _upperBitmapOffset;
            _frameUpdateTimer.AddDuration();
        }
    }
    
    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        Bitmap lowerBitmap = H4D2Art.Commons[_type][_lowerFrame];
        Bitmap upperBitmap = H4D2Art.Commons[_type][_upperFrame];
        screen.Draw(lowerBitmap, xCorrected, yCorrected, _xFlip);
        screen.Draw(upperBitmap, xCorrected, yCorrected, _xFlip);
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