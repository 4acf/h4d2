using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Weapons;

public class GrenadeLauncher : Weapon
{
    public GrenadeLauncher(Level level) : base(level, WeaponConfigs.GrenadeLauncher)
    {

    }

    public override void Shoot(Position position, double directionRadians, bool isBiled = false)
    {
        if (!CanShoot()) return;
        
        (int audioX, int audioY) = Isometric.WorldSpaceToScreenSpace(position.X, position.Y);
        AudioManager.Instance.PlaySFX(_shootSound, audioX, audioY);
        
        AmmoLoaded--;
        _shootDelayTimer.Reset();
        double finalDirection = directionRadians;
        if (isBiled)
        {
            double spread = _CalculateBiledSpread();
            double newXComponent = Math.Cos(directionRadians) + (RandomSingleton.Instance.NextDouble() - 0.5) * spread;
            double newYComponent = Math.Sin(directionRadians) + (RandomSingleton.Instance.NextDouble() - 0.5) * spread;
            finalDirection = MathHelpers.NormalizeRadians(Math.Atan2(newYComponent, newXComponent));
        }
        for (int i = 0; i < _pellets; i++)
        {
            var grenade = new Grenade(_level, position, _damage, finalDirection);
            _level.AddProjectile(grenade);
        }
    }
}