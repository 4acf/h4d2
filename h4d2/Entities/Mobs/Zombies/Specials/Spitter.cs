using H4D2.Entities.Projectiles.ThrowableProjectiles;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Spitter : Special
{
    private const double _attackRange = 100.0;
    private const double _attackDelay = 4.0;
    private const double _footstepDelay = 0.03;
    private const double _spitFreezeTime = 1.0;
    private double _aimDirectionRadians;
    private readonly CountdownTimer _attackDelayTimer;
    private readonly CountdownTimer _footstepParticleTimer;
    private readonly CountdownTimer _spitFreezeTimer;
    private readonly ReadonlyVelocity _nullVelocity = new();
    
    public Spitter(Level level, Position position) 
        : base(level, position, SpecialConfigs.Spitter)
    {
        _aimDirectionRadians = 0.0;
        _attackDelayTimer = new CountdownTimer(_attackDelay);
        _footstepParticleTimer = new CountdownTimer(_footstepDelay);
        _spitFreezeTimer = new CountdownTimer(_spitFreezeTime);
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        _footstepParticleTimer.Update(elapsedTime);
        if (_footstepParticleTimer.IsFinished)
        {
            int randomInt = RandomSingleton.Instance.Next(5);
            if (randomInt != 0)
            {
                var spit = new InvolatileSpit(_level, FootPosition.MutableCopy(), _nullVelocity);
                _level.AddParticle(spit);
            }
            _footstepParticleTimer.Reset();
        }
    }

    protected override void _UpdateAttackState(double elapsedTime)
    {
        _attackDelayTimer.Update(elapsedTime);
        if (_isAttacking)
        {
            _spitFreezeTimer.Update(elapsedTime);
            if(_spitFreezeTimer.IsFinished)
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
            _Spit();
            _attackDelayTimer.Reset();
            _spitFreezeTimer.Reset();
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

    private void _Spit()
    {
        var spit = new SpitProjectile(_level, CenterMass.MutableCopy(), _aimDirectionRadians);
        _level.AddProjectile(spit);
    }
}