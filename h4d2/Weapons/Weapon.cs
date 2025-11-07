using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Weapons;

public abstract class Weapon
{
    public int AmmoLoaded { get; protected set; }
    
    protected readonly Level _level;
    protected readonly int _damage;
    protected readonly double _reloadTimeSeconds;
    protected readonly double _shootDelaySeconds;
    protected readonly int _ammoPerMagazine;
    protected readonly double _spread;
    protected readonly int _pellets;

    protected bool _isReloading = false;

    protected readonly CountdownTimer _reloadTimer;
    protected readonly CountdownTimer _shootDelayTimer;
    
    public double ReloadSecondsLeft { get; protected set; } = 0;
    
    protected double _shootDelaySecondsLeft = 0.0;
    
    protected Weapon(Level level, WeaponConfig config)
    {
        _level = level;
        _damage = config.Damage;
        _reloadTimeSeconds = config.ReloadTimeSeconds;
        _shootDelaySeconds = config.ShootDelaySeconds;
        _ammoPerMagazine = config.AmmoPerMagazine;
        _spread = config.Spread;
        _pellets = config.Pellets;

        AmmoLoaded = config.AmmoPerMagazine;
        _isReloading = false;
        
        _reloadTimer = new CountdownTimer(_reloadTimeSeconds);
        _shootDelayTimer = new CountdownTimer(_shootDelaySeconds);
    }
    
    public void Update(double elapsedTime)
    {
        if (_isReloading)
        {
            ReloadSecondsLeft -= elapsedTime;
            if (ReloadSecondsLeft <= 0)
                _isReloading = false;
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
        if (_isReloading)
            return false;
        if (_shootDelaySecondsLeft > 0)
            return false;
        return true;
    }
    
    public virtual void Shoot(Position position, double directionRadians)
    {
        if (!CanShoot()) return;
        AmmoLoaded--;
        _shootDelaySecondsLeft = _shootDelaySeconds;
        for (int i = 0; i < _pellets; i++)
        {
            double newXComponent = Math.Cos(directionRadians) + (RandomSingleton.Instance.NextDouble() - 0.5) * _spread;
            double newYComponent = Math.Sin(directionRadians) + (RandomSingleton.Instance.NextDouble() - 0.5) * _spread;
            double newDirection = Math.Atan2(newYComponent, newXComponent);
            var bullet = new Bullet(_level, position.Copy(), _damage, newDirection);
            _level.AddProjectile(bullet);
        }
    }

    public void Reload()
    {
        if (_isReloading) return;
        _isReloading = true;
        ReloadSecondsLeft = _reloadTimeSeconds;
        AmmoLoaded = _ammoPerMagazine;
    }
}