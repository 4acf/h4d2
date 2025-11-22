using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities;

public abstract class Entity : Isometric
{
    public readonly BoundingBox BoundingBox;
    public ReadonlyPosition CenterMass => BoundingBox.CenterMass(Position); 
    public ReadonlyPosition FootPosition => BoundingBox.FootPosition(Position);

    public static readonly Comparison<Entity> Comparator = (a, b) =>
    {
        if (a is Survivor sa && sa.Pinner == b)
            return ResolvePinnedSort(b);
        if (b is Survivor sb && sb.Pinner == a)
            return -ResolvePinnedSort(a);
        
        int diff = b.FootPosition.Y.CompareTo(a.FootPosition.Y);
        if (diff != 0)
            return diff;
        
        return 0;
        
        int ResolvePinnedSort(Entity pinner)
        {
            return pinner switch
            {
                Hunter hunter => ResolveHunterSort(hunter),
                Charger charger => ResolveChargerSort(charger),
                _ => 0
            };
        }

        int ResolveHunterSort(Hunter hunter)
        {
            double degrees = MathHelpers.RadiansToDegrees(hunter.DirectionRadians);
            bool drawSurvivorAfterHunter = 45 <= degrees && degrees < 135;
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
    };
    
    protected Entity? _collisionExcludedEntity;
    
    protected Entity(Level level, Position position, BoundingBox boundingBox)
        : base(level, position)
    {
        BoundingBox = boundingBox;
        _collisionExcludedEntity = null;
    }
    
    public abstract void Update(double elapsedTime);

    public bool IsIntersecting(Entity other, ReadonlyPosition position) =>
        BoundingBox.IsIntersecting(other, position);

    public bool Blocks(CollisionGroup otherCollisionGroup)
    {
        return _level.CollisionManager.IsBlockedBy(otherCollisionGroup, BoundingBox.CollisionGroup);
    }
    
    protected void _AttemptMove()
    {
        int steps = (int)(Math.Sqrt(_velocity.HypotenuseSquared) + 1);
        for (int i = 0; i < steps; i++)
        {
            _Move(_velocity.X / steps, 0, 0);
            _Move(0,_velocity.Y / steps, 0);
            _Move(0, 0, _velocity.Z / steps);
        }
    }

    private bool _IsOutOfLevelBounds(ReadonlyPosition position)
    {
        double w = BoundingBox.W(position.X);
        if (w < -Level.Padding) 
            return true;
        
        double s = BoundingBox.S(position.Y);
        if(s < -Level.Padding) 
            return true;
        
        double e = BoundingBox.E(position.X);
        if (e >= _level.Width + Level.Padding) 
            return true;
        
        double n = BoundingBox.N(position.Y);
        if (n >= _level.Height + Level.Padding) 
            return true;

        if (position.Z < 0)
            return true;
        
        return false;
    }
    
    private void _Move(double xComponent, double yComponent, double zComponent)
    {
        var destination = new ReadonlyPosition(
            _position.X + xComponent,
            _position.Y + yComponent,
            _position.Z + zComponent
        );

        if (_IsOutOfLevelBounds(destination))
        {
            if (destination.Z < 0) _position.Z = 0;
            _Collide(null);
            return;
        }

        Entity? collidingEntity = _level.GetFirstCollidingEntity(this, destination, _collisionExcludedEntity);
        if (collidingEntity != null)
        {
            _Collide(collidingEntity);
            
            if(collidingEntity.Blocks(BoundingBox.CollisionGroup))
                return;
        }

        _position.X = destination.X;
        _position.Y = destination.Y;
        _position.Z = destination.Z;
    }
    
    protected virtual void _Collide(Entity? entity)
    {
        _velocity.Stop();
    }
}