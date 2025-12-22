using H4D2.Entities;

namespace H4D2.Levels;

public class EntityCollisionMap
{
    private const int _defaultEntityCap = 16;
    private readonly Level _level;
    private readonly List<Entity>[] _map;
    
    public EntityCollisionMap(Level level)
    {
        _level = level;
        _map = new List<Entity>[_level.Width * _level.Height];
        for (int i = 0; i < _level.Width * _level.Height; i++)
        {
            _map[i] = new List<Entity>(_defaultEntityCap);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _level.Width * _level.Height; i++)
        {
            _map[i].Clear();
        }
    }

    public void AddEntityToTile(Entity entity, Tile tile)
    {
        _map[_level.TileIndex(tile)].Add(entity);
    }
    
    public IEnumerable<Entity> GetTileEntities(Tile tile)
    {
        return _map[_level.TileIndex(tile)];
    }
}