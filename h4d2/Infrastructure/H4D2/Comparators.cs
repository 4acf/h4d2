using H4D2.Entities;
using H4D2.Entities.Hazards;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Zombies.Specials.Pinners;
using H4D2.Entities.Projectiles;
using H4D2.Particles;
using H4D2.Particles.Clouds.Cloudlets;
using H4D2.Particles.DebrisParticles;
using H4D2.Particles.DebrisParticles.Granules;
using H4D2.Particles.Smokes;

namespace H4D2.Infrastructure.H4D2;

public static class Comparators
{
    public static readonly Comparison<Entity> EntityUpdating = (a, b) =>
    {
        int diff = Rank(a.GetType()).CompareTo(Rank(b.GetType()));
        if (diff != 0)
            return diff;
        return b.FootPosition.Y.CompareTo(a.FootPosition.Y);
        
        int Rank(Type t)
        {
            if (typeof(Projectile).IsAssignableFrom(t)) return -1;
            if (typeof(Hazard).IsAssignableFrom(t)) return -1;
            if (typeof(Special).IsAssignableFrom(t)) return 0;
            return 1;
        }
    };
    
    public static readonly Comparison<Entity> EntityRendering = (a, b) =>
    {
        int positionDiff = b.FootPosition.Y.CompareTo(a.FootPosition.Y);
        if (positionDiff != 0) 
            return positionDiff;
        
        if (a is Survivor sa && sa.Pinner == b && b is not Smoker)
            return ResolvePinnedSort(b);
        if (b is Survivor sb && sb.Pinner == a && a is not Smoker)
            return -ResolvePinnedSort(a);

        if (a is Survivor sat && sat.Pinner is Smoker && b is Tongue at)
            return ResolveTongueSort(at);
        if (b is Survivor sbt && sbt.Pinner is Smoker && a is Tongue bt)
            return -ResolveTongueSort(bt);
        
        return 0;
        
        int ResolvePinnedSort(Entity pinner)
        {
            return pinner switch
            {
                Jockey => -1,
                Hunter hunter => ResolveHunterSort(hunter),
                Charger charger => ResolveChargerSort(charger),
                _ => 0
            };
        }

        int ResolveHunterSort(Hunter hunter)
        {
            double degrees = MathHelpers.RadiansToDegrees(hunter.DirectionRadians);
            bool drawSurvivorAfterHunter = 225 <= degrees && degrees < 315;
            if(drawSurvivorAfterHunter)
                return 1;
            return -1;
        }
        
        int ResolveChargerSort(Charger charger)
        {
            bool drawSurvivorAfterCharger = false;

            if (charger.IsCharging || charger.IsStumbling)
            {
                double degrees = MathHelpers.RadiansToDegrees(charger.DirectionRadians);
                bool facingWest = 157.5 <= degrees && degrees < 202.5;
                bool facingEast = 337.5 <= degrees || degrees < 22.5;
                drawSurvivorAfterCharger = 202.5 <= degrees && degrees < 337.5;
                if(charger.IsStumbling && !facingWest && !facingEast)
                    drawSurvivorAfterCharger = !drawSurvivorAfterCharger;
            }
            else
                drawSurvivorAfterCharger = true;
            
            if (drawSurvivorAfterCharger)
                return 1;
            return -1;
        }

        int ResolveTongueSort(Tongue tongue)
        {
            double degrees = MathHelpers.RadiansToDegrees(tongue.DirectionRadians);
            bool drawTongueAfterSurvivor = 67.5 <= degrees && degrees < 112.5;
            if(drawTongueAfterSurvivor)
                return -1;
            return 1;
        }
    };
    
    public static readonly Comparison<Particle> Particle = (a, b) =>
    {
        int diff = Rank(a.GetType()).CompareTo(Rank(b.GetType()));
        if (diff != 0)
            return diff;

        if (a.GetType() == typeof(Flame) && b.GetType() == typeof(Flame))
            return a.Position.Y.CompareTo(b.Position.Y);

        return 0;

        int Rank(Type t)
        {
            if (t == typeof(Fuel)) return 0;
            if (t == typeof(GibDebris)) return 1;
            if (t == typeof(Blood)) return 3;
            if (t == typeof(ExplosionFlame)) return 4;
            if (t == typeof(Flame)) return 5;
            if (t == typeof(Smoke)) return 6;
            return 2;
        }
    };
}