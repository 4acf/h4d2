using H4D2.Levels;

namespace H4D2.Weapons;

public class PumpShotgun : Weapon
{
    public PumpShotgun(Level level) : base(level)
    {
        Damage = 25;
        ReloadTimeSeconds = 3.6;
        ShootDelaySeconds = 1 / 1.5;
        AmmoPerMagazine = 8;
        Spread = 0.5;
        Pellets = 10;
        
        AmmoLoaded = AmmoPerMagazine;
    }    
}