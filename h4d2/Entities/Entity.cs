using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Entities;

public abstract class Entity : Isometric
{
    public readonly BoundingBox BoundingBox;
    public ReadonlyPosition CenterMass => BoundingBox.CenterMass(Position); 
    public ReadonlyPosition FootPosition => BoundingBox.FootPosition(Position);
    
    protected Entity(Level level, Position position, BoundingBox boundingBox)
        : base(level, position)
    {
        BoundingBox = boundingBox;
    }
    
    public abstract void Update(double elapsedTime);

    public bool IsIntersecting(Entity other, ReadonlyPosition position) =>
        BoundingBox.IsIntersecting(other, position);

    public bool IsBlockingEntity(CollisionGroup otherCollisionGroup)
    {
        return _level.CollisionManager.IsBlockedBy(BoundingBox.CollisionGroup, otherCollisionGroup);
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

        Entity? collidingEntity = _level.GetFirstCollidingEntity(this, destination);
        if (collidingEntity != null)
        {
            _Collide(collidingEntity);
            
            if(collidingEntity.IsBlockingEntity(BoundingBox.CollisionGroup))
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