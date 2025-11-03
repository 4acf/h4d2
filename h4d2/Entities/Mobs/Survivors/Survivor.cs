using H4D2.Entities.Mobs.Zombies;
using H4D2.Entities.Pickups;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;

public class Survivor : Mob
{
    public bool IsFullHealth => _health == SurvivorConfigs.DefaultHealth;
    
    private const int _boundaryTolerance = 25;
    private const int _runSpeed = 300;
    private const int _limpSpeed = 150;
    private const int _walkSpeed = 85;
    private const int _adrenalineRunSpeed = 320;
    private const int _adrenalineEffectSeconds = 15;
    private const int _healthBarRed = 0xe61515;
    private const int _healthBarGreen = 0x56de47;
    
    private readonly int _character;
    protected Weapon? _weapon;
    private Zombie? _target;
    private bool _isShooting;
    private double _aimDirectionRadians;
    private bool _isAdrenalineBoosted;
    private double _adrenalineSecondsLeft;
    
    protected Survivor(Level level, Position position, SurvivorConfig config) 
        : base(level, position, config)
    {
        _character = config.Character;
        _target = null;
        _isShooting = false;
        _isAdrenalineBoosted = false;
        _adrenalineSecondsLeft = 0;
        _aimDirectionRadians = 0;
    }

    public override void Update(double elapsedTime)
    {
        _UpdateTarget();
        _UpdateWeapon(elapsedTime);
        _UpdateSpeed(elapsedTime);
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    public void ConsumeFirstAidKit()
    {
        _EmitHealParticles();
        int missingHealth = SurvivorConfigs.DefaultHealth - _health;
        double healthToRestore = 0.8 * missingHealth;
        _health += (int)healthToRestore;
    }

    public void ConsumePills()
    {
        _EmitHealParticles();
        int missingHealth = SurvivorConfigs.DefaultHealth - _health;
        int healthToRestore = Math.Min(50, missingHealth);
        _health += healthToRestore;
    }

    public void ConsumeAdrenaline()
    {
        _EmitHealParticles();
        int missingHealth = SurvivorConfigs.DefaultHealth - _health;
        int healthToRestore = Math.Min(25, missingHealth);
        _health += healthToRestore;
        _isAdrenalineBoosted = true;
        _adrenalineSecondsLeft = _adrenalineEffectSeconds;
    }

    private void _EmitHealParticles()
    {
        var healCloud = new HealCloud(_level, CenterMass.MutableCopy());
        _level.AddParticle(healCloud);
    }
    
    private void _UpdateTarget()
    {
        ReadonlyPosition survivorPosition = CenterMass;
        _target = _level.GetNearestLivingZombie(Position);
        
        if (_target == null || !_target.IsAlive)
        {
            _target = _level.GetNearestLivingZombie(Position);
            if (_target == null) return;
            ReadonlyPosition targetPosition = _target.CenterMass;
            _aimDirectionRadians = Math.Atan2(targetPosition.Y - survivorPosition.Y, targetPosition.X - survivorPosition.X);
            _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
        }
        else
        {
            if (!_target.IsAlive)
            {
                _target = null;
            }
            else
            {
                ReadonlyPosition targetPosition = _target.CenterMass;
                _aimDirectionRadians = Math.Atan2(targetPosition.Y - survivorPosition.Y, targetPosition.X - survivorPosition.X);
                _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
            }
        }
    }
    
    private void _UpdateWeapon(double elapsedTime)
    {
        if (_weapon == null) return;
        _weapon.Update(elapsedTime);
        if (_weapon.CanShoot() && _target != null)
        {
            _weapon.Shoot(CenterMass.MutableCopy(), _aimDirectionRadians);
            _isShooting = true;
        }
        else
        {
            if(_weapon.AmmoLoaded == 0 || _target == null)
                _isShooting = false;
        }
    }
    
    private void _UpdateSpeed(double elapsedTime)
    {
        _adrenalineSecondsLeft -= elapsedTime;
        if (_adrenalineSecondsLeft <= 0)
        {
            _isAdrenalineBoosted = false;
        }

        if (_isAdrenalineBoosted)
        {
            _speed = _adrenalineRunSpeed;
            return;
        }
        
        _speed = _health switch
        {
            >= 40 => _runSpeed,
            > 1 => _limpSpeed,
            _ => _walkSpeed
        };
    }
    
    private void _UpdatePosition(double elapsedTime)
    {
        _xVelocity *= 0.5;
        _yVelocity *= 0.5;

        double targetDirection = _CalculateBestDirection();
        double directionDiff = targetDirection - _directionRadians;
        directionDiff = Math.Atan2(Math.Sin(directionDiff), Math.Cos(directionDiff));
        _directionRadians += directionDiff * (elapsedTime * _turnSpeed);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        
        double moveSpeed = (_speed * _speedFactor) * elapsedTime;
        _xVelocity += Math.Cos(_directionRadians) * moveSpeed;
        _yVelocity += Math.Sin(_directionRadians) * moveSpeed;

        _AttemptMove();
    }
    
    private double _CalculateBestDirection()
    {
        double direction = RandomSingleton.Instance.NextDouble() * (2 * Math.PI);
        direction = CorrectDirectionToAvoidWalls(direction);
        return direction;
    }

    private double CorrectDirectionToAvoidWalls(double direction)
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
    
    private void _UpdateSprite(double elapsedTime)
    {
        _timeSinceLastFrameUpdate += elapsedTime;
        if (_isShooting)
            _UpdateShootingSprite();
        else
            _UpdateRunningSprite();
    }

    private void _UpdateShootingSprite()
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
        
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_xVelocity == 0 && _yVelocity == 0) _walkStep = 0;
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
            _timeSinceLastFrameUpdate -= _frameDuration;
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
        
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            _walkStep = (_walkStep + 1) % 4;
            if (_xVelocity == 0 && _yVelocity == 0) _walkStep = 0;
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
            _timeSinceLastFrameUpdate -= _frameDuration;
        }
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        _RenderHealthBar(screen, xCorrected, yCorrected);
        
        Bitmap lowerBitmap = Art.Survivors[_character][_lowerFrame];
        Bitmap upperBitmap = Art.Survivors[_character][_upperFrame];
        screen.Draw(lowerBitmap, xCorrected, yCorrected, _xFlip);
        screen.Draw(upperBitmap, xCorrected, yCorrected, _xFlip);
    }

    private void _RenderHealthBar(Bitmap screen, int xCorrected, int yCorrected)
    {
        double percentage = _health / (double)SurvivorConfigs.DefaultHealth;
        int greenPixels = (int)Math.Ceiling(percentage * 10);
        
        screen.Fill(
            xCorrected + Art.SpriteSize - 13 + greenPixels,
            yCorrected - 3,
            xCorrected + Art.SpriteSize - 4,
            yCorrected - 3,
            _healthBarRed
        );
        
        screen.Fill(
            xCorrected + Art.SpriteSize - 13,
            yCorrected - 3,
            xCorrected + Art.SpriteSize - 14 + greenPixels,
            yCorrected - 3,
            _healthBarGreen
        );
    }
    
    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(
            xCorrected + Art.SpriteSize - 10,
            yCorrected - Art.SpriteSize - 1,
            xCorrected + Art.SpriteSize - 7,
            yCorrected - Art.SpriteSize - 1,
            Art.ShadowColor,
            Art.ShadowBlend            
        );
    }

    protected override void _Collide(Entity? entity)
    {
        if (entity != null && entity is Pickup pickup)
            pickup.PickUp(this);
        else
            base._Collide(entity);
    }
}