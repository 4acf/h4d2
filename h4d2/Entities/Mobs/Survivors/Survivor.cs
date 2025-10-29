using H4D2.Entities.Mobs.Zombies;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Survivor : Mob
{
    private readonly int _character;
    protected Weapon? _weapon;
    private Zombie? _target;
    private bool _isShooting;
    public double AimDirectionRadians { get; private set; }
    
    protected Survivor(Level level, int character, int xPosition, int yPosition, int color) 
        : base(level, Cfg.BoundingBox, Cfg.DefaultHealth, Cfg.RunSpeed, xPosition, yPosition, color)
    {
        _character = character;
        _target = null;
        _isShooting = false;
        AimDirectionRadians = 0;
    }

    private double _CalculateBestDirection()
    {
        double direction = RandomSingleton.Instance.NextDouble() * (2 * Math.PI);
        direction = CorrectDirectionToAvoidWalls(direction);
        return direction;
    }

    private double CorrectDirectionToAvoidWalls(double direction)
    {
        const int boundaryTolerance = 25;
        var (x, y, _) = BoundingBox.CenterMass(XPosition, YPosition, ZPosition);
        
        if (x < boundaryTolerance)
        {
            if ((Math.PI / 2) < direction && direction < (3 * Math.PI / 2))
            {
                direction = Math.Atan2(Math.Sin(direction), Math.Cos(direction) * -1);
            }
        }
        
        if (y < boundaryTolerance)
        {
            if (direction > Math.PI)
            {
                direction = Math.Atan2(Math.Sin(direction) * -1, Math.Cos(direction));
            }
        }

        if (_level.Width - x < boundaryTolerance)
        {
            if ((3 * Math.PI / 2) < direction || direction < (Math.PI / 2))
            {
                direction = Math.Atan2(Math.Sin(direction), Math.Cos(direction) * -1);
            }
        }

        if (_level.Height - y < boundaryTolerance)
        {
            if (direction < Math.PI)
            {
                direction = Math.Atan2(Math.Sin(direction) * -1, Math.Cos(direction));
            }
        }

        return direction;
    }

    public void _UpdateTarget()
    {
        if (_target == null || !_target.IsAlive())
        {
            _target = _level.GetNearestLivingZombie(XPosition, YPosition);
            if (_target == null) return;
            var (x0, y0, _) = BoundingBox.CenterMass(XPosition, YPosition, ZPosition);
            var (x1, y1, _) = _target.BoundingBox.CenterMass(_target.XPosition, _target.YPosition, _target.ZPosition);
            AimDirectionRadians = Math.Atan2(y1 - y0, x1 - x0);
            AimDirectionRadians = MathHelpers.NormalizeRadians(AimDirectionRadians);
        }
        else
        {
            if (!_target.IsAlive())
            {
                _target = null;
            }
            else
            {
                AimDirectionRadians = Math.Atan2(_target.YPosition - YPosition, _target.XPosition - XPosition);
                AimDirectionRadians = MathHelpers.NormalizeRadians(AimDirectionRadians);
            }
        }
    }

    public void _UpdateWeapon(double elapsedTime)
    {
        if (_weapon == null) return;
        _weapon.Update(elapsedTime);
        if (_weapon.CanShoot() && _target != null)
        {
            _weapon.Shoot();
            _isShooting = true;
        }
        else
        {
            if(_weapon.AmmoLoaded == 0 || _target == null)
                _isShooting = false;
        }
    }
    
    public override void Update(double elapsedTime)
    {
        _UpdateTarget();
        _UpdateWeapon(elapsedTime);
        _UpdateSpeed();
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    private void _UpdateSpeed()
    {
        bool isLimping = _speed < Cfg.WalkSpeed && _speed < Cfg.RunSpeed;
        bool isWalking = _speed < Cfg.LimpSpeed;
        bool isHealthBetween1and39 = 1 < _health && _health < 40;
        
        if (isHealthBetween1and39 && !isLimping)
            _speed = Cfg.LimpSpeed;
        else if(_health == 1 && !isWalking)
            _speed = Cfg.WalkSpeed;
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
        double degrees = MathHelpers.RadiansToDegrees(AimDirectionRadians);
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
        Bitmap lowerBitmap = Art.Survivors[_character][_lowerFrame];
        Bitmap upperBitmap = Art.Survivors[_character][_upperFrame];
        screen.Draw(lowerBitmap, xCorrected, yCorrected, _xFlip);
        screen.Draw(upperBitmap, xCorrected, yCorrected, _xFlip);
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(
            xCorrected + Art.SpriteSize - 10,
            yCorrected - Art.SpriteSize - 1,
            xCorrected + Art.SpriteSize - 7,
            yCorrected - Art.SpriteSize - 1,
            0x0,
            0.9            
        );
    }
    
}