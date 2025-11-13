using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;
using SpitParticle = Particles.DebrisParticles.Granules.Spit;
using SpitProjectile = Spit;

public class Spitter : Special
{
    private const double _attackRange = 100.0;
    private const double _attackDelay = 1.0;
    private const double _footstepDelay = 0.03;
    private double _aimDirectionRadians;
    private readonly CountdownTimer _attackDelayTimer;
    private readonly CountdownTimer _footstepTimer;
    private readonly ReadonlyVelocity _nullVelocity = new();
    
    public Spitter(Level level, Position position) 
        : base(level, position, SpecialConfigs.Spitter)
    {
        _aimDirectionRadians = 0.0;
        _attackDelayTimer = new CountdownTimer(_attackDelay);
        _footstepTimer = new CountdownTimer(_footstepDelay);
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        _footstepTimer.Update(elapsedTime);
        if (_footstepTimer.IsFinished)
        {
            int randomInt = RandomSingleton.Instance.Next(5);
            if (randomInt != 0)
            {
                var spit = new SpitParticle(_level, FootPosition.MutableCopy(), _nullVelocity);
                _level.AddParticle(spit);
            }

            _footstepTimer.Reset();
        }
    }

    protected override void _UpdateAttackState(double elapsedTime)
    {
        _attackDelayTimer.Update(elapsedTime);

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
            _Spit();
            _attackDelayTimer.Reset();
        }
    }

    private void _Spit()
    {
        var spit = new SpitProjectile(_level, CenterMass.MutableCopy(), _aimDirectionRadians);
        _level.AddProjectile(spit);
    }
}