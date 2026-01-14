using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities;

public abstract class Entity : Isometric
{
    public readonly BoundingBox BoundingBox;
    public ReadonlyPosition CenterMass => BoundingBox.CenterMass(Position); 
    public ReadonlyPosition FootPosition => BoundingBox.FootPosition(Position);
    public ReadonlyPosition NWPosition => BoundingBox.NWPosition(Position);
    public ReadonlyPosition NEPosition => BoundingBox.NEPosition(Position);
    public ReadonlyPosition SWPosition => BoundingBox.SWPosition(Position);
    public ReadonlyPosition SEPosition => BoundingBox.SEPosition(Position);
    
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
    
    private void _Move(double xComponent, double yComponent, double zComponent)
    {
        var destination = new ReadonlyPosition(
            _position.X + xComponent,
            _position.Y + yComponent,
            _position.Z + zComponent
        );

        if (destination.Z < 0)
        {
            _Collide(null);
            _position.Z = 0;
            return;
        }
        
        if (_level.IsBlockedByWall(this, destination))
        {
            _CollideWall(xComponent, yComponent, zComponent);
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

    protected virtual void _CollideWall(double xComponent, double yComponent, double zComponent)
    {
        if (xComponent != 0)
            _velocity.X = 0;
        if (yComponent != 0)
            _velocity.Y = 0;
        if (zComponent != 0)
            _velocity.Z = 0;
    }
}