using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Boomer : Special
{
    private const int _pukeFrameOffset = 9;
    private const double _attackRange = 30.0;
    private const double _attackDelay = 30.0;
    private const double _angleVariance = Math.PI / 16.0;
    private const double _pukeFreezeTime = 0.5;
    private const int _numPukeProjectilesPerUpdate = 6;
    private const int _numPukeProjectilesOnDeath = 200;

    private double _aimDirectionRadians;
    private readonly CountdownTimer _attackDelayTimer;
    private readonly CountdownTimer _pukeFreezeTimer;
    
    public Boomer(Level level, Position position) 
        : base(level, position, SpecialConfigs.Boomer)
    {
        _aimDirectionRadians = 0.0;
        _attackDelayTimer = new CountdownTimer(_attackDelay);
        _attackDelayTimer.Update(_attackDelay);
        _pukeFreezeTimer = new CountdownTimer(_pukeFreezeTime);
    }

    public override void Update(double elapsedTime)
    {
        _UpdatePuking();
        base.Update(elapsedTime);
    }

    private void _UpdatePuking()
    {
        if (!_isAttacking)
            return;

        for (int i = 0; i < _numPukeProjectilesPerUpdate; i++)
        {
            double randomDirectionShift = MathHelpers.GaussianRandom(0, _angleVariance);
            double directionRadians = _aimDirectionRadians + randomDirectionShift;
            directionRadians = MathHelpers.NormalizeRadians(directionRadians);
            var puke = new Puke(_level, CenterMass.MutableCopy(), directionRadians);
            _level.AddProjectile(puke);
        }
    }
    
    protected override void _UpdateAttackState(double elapsedTime)
    {
        _attackDelayTimer.Update(elapsedTime);
        if (_isAttacking)
        {
            _pukeFreezeTimer.Update(elapsedTime);
            if(_pukeFreezeTimer.IsFinished)
                _isAttacking = false;
        }
        
        if (_target == null || _target.Removed)
            return;
        
        ReadonlyPosition targetPosition = _target.CenterMass;
        ReadonlyPosition zombiePosition = CenterMass;
        double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);
        
        if (distance > _attackRange)
            return;
        
        _aimDirectionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
        _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
        
        if (_attackDelayTimer.IsFinished)
        {
            _isAttacking = true;
            _attackDelayTimer.Reset();
            _pukeFreezeTimer.Reset();
        }
    }
    
    protected override void _UpdatePosition(double elapsedTime)
    {
        if (_isAttacking)
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
        if (_isAttacking)
        {
            _UpdateAttackSprite(elapsedTime);
        }
        else
        {
            base._UpdateSprite(elapsedTime);
        }
    }

    private void _UpdateAttackSprite(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
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
            _frame = _pukeFrameOffset + direction;
            _frameUpdateTimer.AddDuration();
        }
    }

    protected override void _Die()
    {
        base._Die();
        
        // todo: optimize this!!
        for (int i = 0; i < _numPukeProjectilesOnDeath; i++)
        {
            double randomDouble = RandomSingleton.Instance.NextDouble();
            double directionRadians = randomDouble * (Math.PI * 2);
            var puke = new DeathPuke(_level, CenterMass.MutableCopy(), directionRadians);
            _level.AddProjectile(puke);
        }
    }
}