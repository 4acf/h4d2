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
        ShootDelaySeconds = 1;
        AmmoPerMagazine = 1;
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
            (double x, double y, double z) = _owner.CenterMass; 
            _level.AddProjectile(new Grenade(_level, x, y, z, Damage, _owner.AimDirectionRadians));
        }
    }
}