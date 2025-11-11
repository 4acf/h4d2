namespace H4D2.Infrastructure.H4D2;

public enum CollisionGroup
{
    Survivor,
    Zombie,
    Projectile,
    Pickup,
    Hazard
}

public static class Collisions {
    public static void Configure(CollisionManager<CollisionGroup> collisionManager)
    {
        collisionManager.AddOneWayBlockingCollision(CollisionGroup.Survivor, CollisionGroup.Zombie);
        collisionManager.AddOneWayNonBlockingCollision(CollisionGroup.Survivor, CollisionGroup.Pickup);
        collisionManager.AddOneWayNonBlockingCollision(CollisionGroup.Survivor, CollisionGroup.Hazard);
        collisionManager.AddOneWayBlockingCollision(CollisionGroup.Zombie, CollisionGroup.Survivor);
        collisionManager.AddOneWayNonBlockingCollision(CollisionGroup.Zombie, CollisionGroup.Hazard);
        collisionManager.AddOneWayBlockingCollision(CollisionGroup.Projectile, CollisionGroup.Zombie);
    }
}