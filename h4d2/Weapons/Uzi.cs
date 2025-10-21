using H4D2.Entities.Mobs.Survivors;
using H4D2.Levels;

namespace H4D2.Weapons;

public class Uzi : Weapon
{
    public Uzi(Level level, Survivor owner) : base(level, owner)
    {
        Damage = 20;
        ReloadTimeSeconds = 2.27;
        ShootDelaySeconds = 1.0 / 16.0;
        AmmoPerMagazine = 50;
        ShootsTheFloor = false;
        MaxRange = -1;
        Spread = 0.05;
        Pellets = 1;
        
        AmmoLoaded = AmmoPerMagazine;
    }    
}