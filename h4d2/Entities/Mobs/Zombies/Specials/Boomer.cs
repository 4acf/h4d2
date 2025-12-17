using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Boomer : Special
{
    private const int _pukeFrameOffset = 9;
    private const double _attackRange = 30.0;
    private const double _attackDelay = 30.0;
    private const double _angleVariance = Math.PI / 16.0;
    private const double _pukeFreezeTime = 0.5;
    private const int _numPukeProjectilesPerUpdate = 6;
    private const int _bileGibs = 5;
    private const double _splashRadius = 25.0;
    
    private readonly CountdownTimer _attackDelayTimer;
    private readonly CountdownTimer _pukeFreezeTimer;
    
    public Boomer(Level level, Position position) 
        : base(level, position, SpecialConfigs.Boomer)
    {
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
            double directionRadians = _directionRadians + randomDirectionShift;
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
        
        if (distance > _attackRange || !_pathfinder.HasLineOfSight(_target))
            return;
        
        _directionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
        _directionRadians = MathHelpers.NormalizeRadians(_directionRadians);
        
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

        SpriteDirection spriteDirection = Direction.Intercardinal(_directionRadians);
        _xFlip = spriteDirection.XFlip;
        
        while (_frameUpdateTimer.IsFinished)
        {
            _frame = _pukeFrameOffset + spriteDirection.Offset;
            _frameUpdateTimer.AddDuration();
        }
    }

    protected override void _Die()
    {
        base._Die();
        _level.ExplodeBile(this, _splashRadius);
        for (int i = 0; i < _bileGibs; i++)
        {
            Position position = CenterMass.MutableCopy();
            position.Z += i;
            var deathSplatter = new BileGibDebris(_level, position, _gibColor);
            _level.AddParticle(deathSplatter);
        }
    }
}