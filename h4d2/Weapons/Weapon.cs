using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles;
using H4D2.Levels;

namespace H4D2.Weapons;

public abstract class Weapon
{
    public double Damage { get; init; }
    public double ReloadTimeSeconds { get; init; }
    public double ShootDelaySeconds { get; init; }
    public int AmmoPerMagazine { get; init; }
    public bool ShootsTheFloor { get; init; }
    public double MaxRange { get; init; }
    public double Spread { get; init; }
    public int Pellets { get; init; }

    public bool IsReloading { get; protected set; } = false;

    public int AmmoLoaded { get; protected set; }

    public double ReloadSecondsLeft { get; protected set; } = 0;
    
    protected double _shootDelaySecondsLeft = 0;

    private Level _level;
    private Survivor _owner;
    
    public Weapon(Level level, Survivor owner)
    {
        _level = level;
        _owner = owner;
    }
    
    public void Update(double elapsedTime)
    {
        if (IsReloading)
        {
            ReloadSecondsLeft -= elapsedTime;
            if (ReloadSecondsLeft <= 0)
                IsReloading = false;
        }
        else
        {
            _shootDelaySecondsLeft -= elapsedTime;
            if (AmmoLoaded == 0 && _shootDelaySecondsLeft <= 0)
            {
                Reload();
            }
        }
    }
    
    public bool CanShoot()
    {
        if (AmmoLoaded == 0)
            return false;
        if (IsReloading)
            return false;
        if (_shootDelaySecondsLeft > 0)
            return false;
        return true;
    }
    
    public void Shoot()
    {
        if (!CanShoot()) return;
        AmmoLoaded--;
        _shootDelaySecondsLeft = ShootDelaySeconds;
        for (int i = 0; i < Pellets; i++)
        {
            var (x, y) = _owner.BoundingBox.CenterMass(_owner.XPosition, _owner.YPosition); 
            _level.AddBullet(new Bullet(_level, _owner.AimDirectionRadians, x, y));
        }
    }

    public void Reload()
    {
        if (IsReloading) return;
        IsReloading = true;
        ReloadSecondsLeft = ReloadTimeSeconds;
        AmmoLoaded = AmmoPerMagazine;
    }
}