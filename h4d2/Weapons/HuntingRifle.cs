using H4D2.Entities.Mobs.Survivors;
using H4D2.Levels;

namespace H4D2.Weapons;

public class HuntingRifle : Weapon
{
    public HuntingRifle(Level level) : base(level)
    {
        Damage = 90;
        ReloadTimeSeconds = 3.13;
        ShootDelaySeconds = 1.0 / 4.0;
        AmmoPerMagazine = 15;
        Spread = 0;
        Pellets = 1;
        
        AmmoLoaded = AmmoPerMagazine;
    }    
}