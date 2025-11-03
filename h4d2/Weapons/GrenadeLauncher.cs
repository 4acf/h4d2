using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Weapons;

public class GrenadeLauncher : Weapon
{
    public GrenadeLauncher(Level level) : base(level)
    {
        Damage = 400;
        ReloadTimeSeconds = 0;
        ShootDelaySeconds = 3.3;
        AmmoPerMagazine = 2;
        Spread = 0;
        Pellets = 1;
        
        AmmoLoaded = AmmoPerMagazine;
    }

    public override void Shoot(Position position, double directionRadians)
    {
        if (!CanShoot()) return;
        AmmoLoaded--;
        _shootDelaySecondsLeft = ShootDelaySeconds;
        for (int i = 0; i < Pellets; i++)
        {
            var grenade = new Grenade(_level, position, Damage, directionRadians);
            _level.AddProjectile(grenade);
        }
    }
}