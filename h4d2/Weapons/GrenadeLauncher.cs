using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Weapons;

public class GrenadeLauncher : Weapon
{
    public GrenadeLauncher(Level level) : base(level, WeaponConfigs.GrenadeLauncher)
    {

    }

    public override void Shoot(Position position, double directionRadians)
    {
        if (!CanShoot()) return;
        AmmoLoaded--;
        _shootDelaySecondsLeft = _shootDelaySeconds;
        for (int i = 0; i < _pellets; i++)
        {
            var grenade = new Grenade(_level, position, _damage, directionRadians);
            _level.AddProjectile(grenade);
        }
    }
}