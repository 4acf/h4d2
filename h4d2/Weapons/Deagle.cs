using H4D2.Levels;

namespace H4D2.Weapons;

public class Deagle : Weapon
{
    public Deagle(Level level) : base(level)
    {
        Damage = 80;
        ReloadTimeSeconds = 2.0;
        ShootDelaySeconds = 1 / 3.33;
        AmmoPerMagazine = 8;
        Spread = 0.1;
        Pellets = 1;
        
        AmmoLoaded = AmmoPerMagazine;
    }
}