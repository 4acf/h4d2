namespace H4D2.Infrastructure;

public class CollisionManager<T> where T : Enum
{
    private struct Collidable
    {
        public readonly int Mask;
        public int CollidesWith;
        public int IsBlockedBy;
        
        public Collidable(int mask)
        {
            Mask = mask;
            CollidesWith = 0;
            IsBlockedBy = 0;
        }
    }
    
    private readonly Dictionary<T, Collidable> _collidables;
    
    public CollisionManager()
    {
        _collidables = new Dictionary<T, Collidable>();
        int i = 1;
        foreach (T key in Enum.GetValues(typeof(T)))
        {
            if (i >= int.MaxValue / 2)
                throw new OverflowException("Max collidable objects reached");
            
            _collidables[key] = new Collidable(i);
            i *= 2;
        }
    }

    public void AddOneWayBlockingCollision(T key1, T key2)
    {
        Collidable collidable1 = _collidables[key1];
        Collidable collidable2 = _collidables[key2];
        
        collidable1.CollidesWith |= collidable2.Mask;
        collidable1.IsBlockedBy |= collidable2.Mask;
        _collidables[key1] = collidable1;
    }
    
    public void AddOneWayNonBlockingCollision(T key1, T key2)
    {
        Collidable collidable1 = _collidables[key1];
        Collidable collidable2 = _collidables[key2];
        
        collidable1.CollidesWith |= collidable2.Mask;
        _collidables[key1] = collidable1;
    }

    public bool CanCollideWith(T key1, T key2)
    {
        Collidable c1 = _collidables[key1];
        Collidable c2 = _collidables[key2];
        
        return (c1.CollidesWith & c2.Mask) == c2.Mask
               && (c1.CollidesWith & c2.Mask) != 0;
    }

    public bool IsBlockedBy(T key1, T key2)
    {
        Collidable c1 = _collidables[key1];
        Collidable c2 = _collidables[key2];
        
        return (c1.IsBlockedBy & c2.Mask) == c2.Mask
               && (c1.IsBlockedBy & c2.Mask) != 0;
    }
}