namespace H4D2.Infrastructure;

public class CollisionManager
{
    private struct Collidable
    {
        public readonly int Mask;
        public int CollidesWith;

        public Collidable(int mask)
        {
            Mask = mask;
            CollidesWith = 0;
        }
    }
    
    private readonly Dictionary<string, Collidable> _collidables;
    
    public CollisionManager(IEnumerable<string> collidableObjects)
    {
        _collidables = new Dictionary<string, Collidable>();
        int i = 1;
        foreach (string collidableObject in collidableObjects)
        {
            if (i >= int.MaxValue / 2)
                throw new OverflowException("Max collidable objects reached");
            
            var collidable = new Collidable(i);
            _collidables.Add(collidableObject, collidable);
            i *= 2;
        }
    }

    public void AddOneWayCollision(string key1, string key2)
    {
        if (!_collidables.TryGetValue(key1, out Collidable collidable1))
            return;

        if (!_collidables.TryGetValue(key2, out Collidable collidable2))
            return;

        collidable1.CollidesWith |= collidable2.Mask;
    }

    public int GetCollisionMask(string key)
    {
        if(!_collidables.TryGetValue(key, out Collidable collidable))
            return 0;
        return collidable.Mask;
    }

    public int GetCollidesWith(string key)
    {
        if (!_collidables.TryGetValue(key, out Collidable collidable))
            return 0;
        return collidable.CollidesWith;
    }
    
    public static bool CanCollideWith(int collidesWith, int otherMask)
    {
        return (collidesWith & otherMask) == otherMask
            && (collidesWith & otherMask) != 0;
    }
}