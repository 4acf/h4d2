using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles;
using H4D2.Levels;

namespace H4D2.Weapons;

public class GrenadeLauncher : Weapon
{
    public GrenadeLauncher(Level level, Survivor owner) : base(level, owner)
    {
        Damage = 400;
        ReloadTimeSeconds = 3.3;
        ShootDelaySeconds = 3.3;
        AmmoPerMagazine = 2;
        Spread = 0;
        Pellets = 1;
        
        AmmoLoaded = AmmoPerMagazine;
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;
        AmmoLoaded--;
        _shootDelaySecondsLeft = ShootDelaySeconds;
        for (int i = 0; i < Pellets; i++)
        {
            var grenade = new Grenade(_level, _owner.CenterMass.MutableCopy(), Damage, _owner.AimDirectionRadians);
            _level.AddProjectile(grenade);
        }
    }
}