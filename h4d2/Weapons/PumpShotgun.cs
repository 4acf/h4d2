using H4D2.Entities.Mobs.Survivors;
using H4D2.Levels;

namespace H4D2.Weapons;

public class PumpShotgun : Weapon
{
    public PumpShotgun(Level level, Survivor owner) : base(level, owner)
    {
        Damage = 25;
        ReloadTimeSeconds = 4.2;
        ShootDelaySeconds = 1;
        AmmoPerMagazine = 8;
        ShootsTheFloor = false;
        MaxRange = -1;
        Spread = 0.05;
        Pellets = 10;
        
        AmmoLoaded = AmmoPerMagazine;
    }    
}