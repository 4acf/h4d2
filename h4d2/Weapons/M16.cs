using H4D2.Entities.Mobs.Survivors;
using H4D2.Levels;

namespace H4D2.Weapons;

public class M16 : Weapon
{
    public M16(Level level, Survivor owner) : base(level, owner)
    {
        Damage = 33;
        ReloadTimeSeconds = 2.2;
        ShootDelaySeconds = 1 / 11.43;
        AmmoPerMagazine = 30;
        ShootsTheFloor = false;
        MaxRange = -1;
        Spread = 0.05;
        Pellets = 1;
        
        AmmoLoaded = AmmoPerMagazine;
    }
}