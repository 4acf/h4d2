using H4D2.Levels;

namespace H4D2.Weapons;

public class Uzi : Weapon
{
    public Uzi(Level level) : base(level)
    {
        Damage = 20;
        ReloadTimeSeconds = 2.27;
        ShootDelaySeconds = 1.0 / 16.0;
        AmmoPerMagazine = 50;
        Spread = 0.4;
        Pellets = 1;
        
        AmmoLoaded = AmmoPerMagazine;
    }    
}