using H4D2.Entities;
using H4D2.Entities.Hazards;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Zombies.Specials.Pinners;
using H4D2.Entities.Projectiles;
using H4D2.Levels;
using H4D2.Levels.LevelElements;
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

    public static readonly Comparison<Isometric> IsometricRendering = (a, b) =>
    {
        ReadonlyPosition aPos = a.Position;
        ReadonlyPosition bPos = b.Position;
        if (a is Entity entity)
            aPos = entity.FootPosition;
        if (b is Entity entity2)
            bPos = entity2.FootPosition;
        if (a is LevelElement)
        {
            aPos = new ReadonlyPosition(
                aPos.X - Level.TilePhysicalOffset.Item1, 
                aPos.Y - Level.TilePhysicalOffset.Item2, 
                aPos.Z
            );
        }
        if (b is LevelElement)
        {
            bPos = new ReadonlyPosition(
                bPos.X - Level.TilePhysicalOffset.Item1, 
                bPos.Y - Level.TilePhysicalOffset.Item2, 
                bPos.Z
            );
        }

        if (a is Flame af)
            aPos = af.FootPosition;
        if (b is Flame bf)
            bPos = bf.FootPosition;
        
        if (aPos.Y + aPos.X < bPos.Y + bPos.X) return 1;
        if (aPos.Y + aPos.X > bPos.Y + bPos.X) return -1;
        if (aPos.Z < bPos.Z) return 1;
        if (aPos.Z > bPos.Z) return -1;
        if (aPos.X < bPos.X) return 1;
        if (aPos.X > bPos.X) return -1;
        if (aPos.Y < bPos.Y) return 1;
        if (aPos.Y > bPos.Y) return -1;
        
        if (a.GetType() == b.GetType())
        {
            return a switch
            {
                Entity ae when b is Entity be => EntityRendering!(ae, be),
                Particle ap when b is Particle bp => ParticleRendering!(ap, bp),
                LevelElement ale when b is LevelElement ble => LevelElementRendering!(ale, ble),
                _ => 0
            };
        }
        
        return TypeRank(a).CompareTo(TypeRank(b));
        
        int TypeRank(Isometric isometric)
        {
            return isometric switch
            {
                Particle => 0,
                Entity => 1,
                LevelElement => 2,
                _ => 3
            };
        }
    };
    
    public static readonly Comparison<Entity> EntityRendering = (a, b) =>
    {
        int positionDiff = b.FootPosition.Y.CompareTo(a.FootPosition.Y);
        if (positionDiff != 0) 
            return positionDiff;

        bool aIsSurvivor = a is Survivor;
        bool bIsSurvivor = b is Survivor;
        if (aIsSurvivor == bIsSurvivor)
            return 0;
        
        int rankA = ResolveSort(a);
        int rankB = ResolveSort(b);

        int rankDiff = rankA.CompareTo(rankB);
        if (rankDiff != 0)
            return rankDiff;
        
        return 0;
        
        int ResolveSort(Entity entity)
        {
            return entity switch
            {
                Jockey => 1,
                Hunter hunter => ResolveHunterSort(hunter),
                Charger charger => ResolveChargerSort(charger),
                Tongue tongue => ResolveTongueSort(tongue),
                _ => 0
            };
        }

        int ResolveHunterSort(Hunter hunter)
        {
            double degrees = MathHelpers.RadiansToDegrees(hunter.DirectionRadians);
            bool drawSurvivorAfterHunter = 225 <= degrees && degrees < 315;
            if(drawSurvivorAfterHunter)
                return -1;
            return 1;
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
                return -1;
            return 1;
        }

        int ResolveTongueSort(Tongue tongue)
        {
            double degrees = MathHelpers.RadiansToDegrees(tongue.DirectionRadians);
            bool drawTongueAfterSurvivor = 67.5 <= degrees && degrees < 112.5;
            if(drawTongueAfterSurvivor)
                return 1;
            return -1;
        }
    };
    
    public static readonly Comparison<Particle> ParticleRendering = (a, b) =>
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

    public static readonly Comparison<LevelElement> LevelElementRendering = (a, b) =>
    {
        ReadonlyPosition aPos = a.Position;
        ReadonlyPosition bPos = b.Position;
        
        if (aPos.Y + aPos.X < bPos.Y + bPos.X) return 1;
        if (aPos.Y + aPos.X > bPos.Y + bPos.X) return -1;
        if (aPos.Z < bPos.Z) return 1;
        if (aPos.Z > bPos.Z) return -1;
        if (aPos.X < bPos.X) return 1;
        if (aPos.X > bPos.X) return -1;
        if (aPos.Y < bPos.Y) return 1;
        if (aPos.Y > bPos.Y) return -1;
        return 0;
    };
}