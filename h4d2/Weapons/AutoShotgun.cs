using H4D2.Entities.Mobs.Survivors;
using H4D2.Levels;

namespace H4D2.Weapons;

public class AutoShotgun : Weapon
{
    public AutoShotgun(Level level, Survivor owner) : base(level, owner)
    {
        Damage = 23;
        ReloadTimeSeconds = 4;
        ShootDelaySeconds =  1 / 3.33;
        AmmoPerMagazine = 10;
        Spread = 0.5;
        Pellets = 11;
        
        AmmoLoaded = AmmoPerMagazine;
    }
}