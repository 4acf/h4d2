using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;

public class Worker : Uncommon
{
    public Worker(Level level, Position position) 
        : base(level, position, UncommonConfigs.Worker)
    {
        
    }    
    
    protected override void _UpdateTarget()
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
                    _target = _level.GetNearestLivingZombie(Position, this);
                }
            }
            return;
        }

        BileBombProjectile? activeBileBomb = _level.GetNearestActiveBileBomb(Position);
        if (activeBileBomb != null)
        {
            _target = activeBileBomb;
            _bileBombTarget = activeBileBomb;
            return;
        }
        
        _target = _level.GetNearestLivingSurvivor(Position);
    }
}