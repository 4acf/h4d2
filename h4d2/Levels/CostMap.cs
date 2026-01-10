using System.Collections.Immutable;

namespace H4D2.Levels;

public class CostMap
{
    private const int _costScale = 5;
    private readonly Level _level;
    private readonly int[][] _internalMap;
    
    public CostMap(Level level, ImmutableArray<TileType> tileTypes)
    {
        _level = level;
        int numTiles = level.Width * level.Height;
        _internalMap = new int[numTiles][];

        var directions = new (int dx, int dy)[]
        {
            ( 0, -1), // N
            ( 1, -1), // NE
            ( 1,  0), // E
            ( 1,  1), // SE
            ( 0,  1), // S
            (-1,  1), // SW
            (-1,  0), // W
            (-1, -1)  // NW
        };

        for (int i = 0; i < numTiles; i++)
        {
            if (tileTypes[i] == TileType.Wall || tileTypes[i] == TileType.EdgeWall)
                continue;
            
            int x = i % level.Width;
            int y = i / level.Width;

            var adjFloors = new List<int>();

            for (int j = 0; j < directions.Length; j++)
            {
                int newX = x + directions[j].dx;
                int newY = y + directions[j].dy;
                if (!level.IsWall(newX, newY))
                {
                    adjFloors.Add(_EncodeTile(level, newX, newY));
                }
            }
            
            _internalMap[i] = adjFloors.ToArray();
        }
    }

    public AdjacentNode[] GetAdjacentNodes(int index)
    {
        int[] adj = _internalMap[index];
        var result = new AdjacentNode[adj.Length];
        for (int i = 0; i < adj.Length; i++)
        {
            int idx = adj[i] >> 1;
            int cost = ((adj[i] & 0b1) * _costScale) + 1;
            result[i] = new AdjacentNode(idx, cost);
        }
        return result;
    }

    public AdjacentNode[] GetAdjacentNodes(int x, int y) => GetAdjacentNodes((y * _level.Width) + x);

    private static int _EncodeTile(Level level, int x, int y)
    {
        return 
            (level.TileIndex(x, y) << 1) | 
            (level.IsTileAdjacentToWall(x, y) ? 0b1 : 0b0);
    }
}

public readonly struct AdjacentNode(int index, int cost)
{
    public readonly int Index = index;
    public readonly int Cost = cost;
}