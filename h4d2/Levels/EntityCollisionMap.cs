using H4D2.Entities;

namespace H4D2.Levels;

public class EntityCollisionMap
{
    private const int _defaultEntityCap = 16;
    private readonly Level _level;
    private readonly List<Entity>[] _map;
    private int[] _generations;
    private int _currentGeneration;
    
    public EntityCollisionMap(Level level)
    {
        int numTiles = level.Width * level.Height;
        
        _level = level;
        _map = new List<Entity>[numTiles];
        _generations = new int[numTiles];
        for (int i = 0; i < numTiles; i++)
        {
            _map[i] = new List<Entity>(_defaultEntityCap);
            _generations[i] = -1;
        }
        _currentGeneration = 0;
    }

    public void Update()
    {
        _currentGeneration++;
    }
    
    public void AddEntityToTile(Entity entity, Tile tile)
    {
        int tileIndex = _level.TileIndex(tile);
        if (_generations[tileIndex] != _currentGeneration)
        {
            _map[tileIndex].Clear();
            _generations[tileIndex] = _currentGeneration;
        }
        _map[tileIndex].Add(entity);
    }
    
    public IEnumerable<Entity> GetTileEntities(Tile tile)
    {
        int tileIndex = _level.TileIndex(tile);
        if(_generations[tileIndex] != _currentGeneration)
            return [];
        return _map[tileIndex];
    }
}