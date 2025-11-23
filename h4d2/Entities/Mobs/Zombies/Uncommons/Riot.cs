using H4D2.Entities.Projectiles;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Emitters;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;

public class Riot : Uncommon
{
    private const double _damageMultiplier = 0.25;
    
    public Riot(Level level, Position position) 
        : base(level, position, UncommonConfigs.Riot)
    {
        
    }
    
    public override void HitBy(Projectile projectile)
    {
        double lowerBound = MathHelpers.NormalizeRadians(DirectionRadians - (Math.PI / 2));
        double upperBound = MathHelpers.NormalizeRadians(DirectionRadians + (Math.PI / 2));

        bool hitInFront = false;

        if (upperBound > lowerBound)
        {
            hitInFront = 
                projectile.DirectionRadians < lowerBound || 
                projectile.DirectionRadians > upperBound;
        }
        else
        {
            hitInFront = 
                upperBound <= projectile.DirectionRadians &&
                projectile.DirectionRadians < lowerBound;
        }
        
        if (!hitInFront)
        {
            base.HitBy(projectile);
            return;
        }
        
        if (Removed || projectile.Removed)
            return;
        _health -= (int)(projectile.Damage * _damageMultiplier);
        if (!IsAlive)
        {
            _Die();
        }
        var bloodSplatter = new BloodSplatter(_level, CenterMass.MutableCopy());
        _level.AddParticle(bloodSplatter);
    }
}