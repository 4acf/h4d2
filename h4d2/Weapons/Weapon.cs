using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
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

    protected readonly Level _level;
    protected readonly Survivor _owner;
    
    protected Weapon(Level level, Survivor owner)
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
    
    public virtual void Shoot()
    {
        if (!CanShoot()) return;
        AmmoLoaded--;
        _shootDelaySecondsLeft = ShootDelaySeconds;
        for (int i = 0; i < Pellets; i++)
        {
            double newXComponent = Math.Cos(_owner.AimDirectionRadians) + (RandomSingleton.Instance.NextDouble() * Spread);
            double newYComponent = Math.Sin(_owner.AimDirectionRadians) + (RandomSingleton.Instance.NextDouble() * Spread);
            double newDirection = Math.Atan2(newYComponent, newXComponent);
            var (x, y) = _owner.BoundingBox.CenterMass(_owner.XPosition, _owner.YPosition); 
            _level.AddProjectile(new Bullet(_level, newDirection, x, y));
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