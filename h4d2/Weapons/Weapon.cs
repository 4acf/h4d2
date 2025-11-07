using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Weapons;

public abstract class Weapon
{
    public int AmmoLoaded { get; protected set; }
    
    protected readonly Level _level;
    protected readonly int _damage;
    protected readonly int _ammoPerMagazine;
    protected readonly double _spread;
    protected readonly int _pellets;

    protected bool _isReloading = false;

    protected readonly CountdownTimer _reloadTimer;
    protected readonly CountdownTimer _shootDelayTimer;
    
    protected Weapon(Level level, WeaponConfig config)
    {
        _level = level;
        _damage = config.Damage;
        _ammoPerMagazine = config.AmmoPerMagazine;
        _spread = config.Spread;
        _pellets = config.Pellets;

        AmmoLoaded = config.AmmoPerMagazine;
        _isReloading = false;
        
        _reloadTimer = new CountdownTimer(config.ReloadTimeSeconds);
        _shootDelayTimer = new CountdownTimer(config.ShootDelaySeconds);
    }
    
    public void Update(double elapsedTime)
    {
        if (_isReloading)
        {
            _reloadTimer.Update(elapsedTime);
            if (_reloadTimer.IsFinished)
                _isReloading = false;
        }
        else
        {
            _shootDelayTimer.Update(elapsedTime);
            if (AmmoLoaded == 0 && _shootDelayTimer.IsFinished)
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
        if (!_shootDelayTimer.IsFinished)
            return false;
        return true;
    }
    
    public virtual void Shoot(Position position, double directionRadians)
    {
        if (!CanShoot()) return;
        AmmoLoaded--;
        _shootDelayTimer.Reset();
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
        _reloadTimer.Reset();
        AmmoLoaded = _ammoPerMagazine;
    }
}