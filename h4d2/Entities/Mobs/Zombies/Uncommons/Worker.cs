using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;

public class Worker : Uncommon
{
    public Worker(Level level, Position position) 
        : base(level, position, UncommonConfigs.Worker)
    {
        
    }    
    
    protected override void _UpdateTarget(double elapsedTime)
    {
        _attackDelaySecondsLeft -= elapsedTime;
        _attackDelaySecondsLeft -= elapsedTime;
        
        if (_target == null || _target.Removed)
        {
            _isAttacking = false;
            _target = _level.GetNearestLivingSurvivor(Position);
        }
        else
        {
            ReadonlyPosition targetPosition = _target.CenterMass;
            ReadonlyPosition zombiePosition = CenterMass;
            double distance = ReadonlyPosition.Distance(targetPosition, zombiePosition);

            _isAttacking = distance <= _attackRange;
            if (!_isAttacking) return;
            
            if (_target is Mob targetMob && _attackDelaySecondsLeft <= 0)
            {
                targetMob.HitBy(this);
                _attackDelaySecondsLeft = _attackDelay;
            }
                
            _aimDirectionRadians = Math.Atan2(targetPosition.Y - zombiePosition.Y, targetPosition.X - zombiePosition.X);
            _aimDirectionRadians = MathHelpers.NormalizeRadians(_aimDirectionRadians);
        }
    }
}