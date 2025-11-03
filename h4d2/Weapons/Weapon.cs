using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Weapons;

public abstract class Weapon
{
    public int Damage { get; init; }
    public double ReloadTimeSeconds { get; init; }
    public double ShootDelaySeconds { get; init; }
    public int AmmoPerMagazine { get; init; }
    public double Spread { get; init; }
    public int Pellets { get; init; }

    public bool IsReloading { get; protected set; } = false;

    public int AmmoLoaded { get; protected set; }

    public double ReloadSecondsLeft { get; protected set; } = 0;
    
    protected double _shootDelaySecondsLeft = 0;

    protected readonly Level _level;
    
    protected Weapon(Level level)
    {
        _level = level;
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
    
    public virtual void Shoot(Position position, double directionRadians)
    {
        if (!CanShoot()) return;
        AmmoLoaded--;
        _shootDelaySecondsLeft = ShootDelaySeconds;
        for (int i = 0; i < Pellets; i++)
        {
            double newXComponent = Math.Cos(directionRadians) + (RandomSingleton.Instance.NextDouble() - 0.5) * Spread;
            double newYComponent = Math.Sin(directionRadians) + (RandomSingleton.Instance.NextDouble() - 0.5) * Spread;
            double newDirection = Math.Atan2(newYComponent, newXComponent);
            var bullet = new Bullet(_level, position.Copy(), Damage, newDirection);
            _level.AddProjectile(bullet);
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