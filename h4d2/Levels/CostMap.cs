using System.Collections.Immutable;

namespace H4D2.Levels;

public class CostMap
{
    private readonly Level _level;
    private readonly int[][] _internalMap;
    
    public CostMap(Level level, ImmutableArray<TileType> tileTypes)
    {
        _level = level;
        int numTiles = level.Width * level.Height;
        _internalMap = new int[numTiles][];
        for (int i = 0; i < numTiles; i++)
        {
            if (tileTypes[i] == TileType.Wall || tileTypes[i] == TileType.EdgeWall)
                continue;
            int numAdjFloors = 0;
            int x = i % level.Width;
            int y = i / level.Width;
            
            bool canMoveN = !level.IsWall(x, y - 1);
            bool canMoveNE = !level.IsWall(x + 1, y - 1);
            bool canMoveE = !level.IsWall(x + 1, y);
            bool canMoveSE = !level.IsWall(x + 1, y + 1);
            bool canMoveS = !level.IsWall(x, y + 1);
            bool canMoveSW = !level.IsWall(x - 1, y + 1);
            bool canMoveW = !level.IsWall(x - 1, y);
            bool canMoveNW = !level.IsWall(x - 1, y - 1);
            
            if (canMoveN) numAdjFloors++;
            if (canMoveNE) numAdjFloors++;
            if (canMoveE) numAdjFloors++;
            if (canMoveSE) numAdjFloors++;
            if (canMoveS) numAdjFloors++;
            if (canMoveSW) numAdjFloors++;
            if (canMoveW) numAdjFloors++;
            if (canMoveNW) numAdjFloors++;
            
            var adjFloors = new int[numAdjFloors];
            int placed = 0;
            if (canMoveN) adjFloors[placed++] = 
                (level.TileIndex(x, y - 1) << 1) | (level.IsTileAdjacentToWall(x, y - 1) ? 0b1 : 0b0);
            if (canMoveNE) adjFloors[placed++] =
                (level.TileIndex(x + 1, y - 1) << 1) | (level.IsTileAdjacentToWall(x + 1, y - 1) ? 0b1 : 0b0);
            if (canMoveE) adjFloors[placed++] =
                (level.TileIndex(x + 1, y) << 1) | (level.IsTileAdjacentToWall(x + 1, y) ? 0b1 : 0b0);
            if (canMoveSE) adjFloors[placed++] =
                (level.TileIndex(x + 1, y + 1) << 1) | (level.IsTileAdjacentToWall(x + 1, y + 1) ? 0b1 : 0b0);
            if (canMoveS) adjFloors[placed++] =
                (level.TileIndex(x, y + 1) << 1) | (level.IsTileAdjacentToWall(x, y + 1) ? 0b1 : 0b0);
            if (canMoveSW) adjFloors[placed++] =
                (level.TileIndex(x - 1, y + 1) << 1) | (level.IsTileAdjacentToWall(x - 1, y + 1) ? 0b1 : 0b0);
            if (canMoveW) adjFloors[placed++] =
                (level.TileIndex(x - 1, y) << 1) | (level.IsTileAdjacentToWall(x - 1, y) ? 0b1 : 0b0);
            if (canMoveNW) adjFloors[placed++] =
                (level.TileIndex(x - 1, y - 1) << 1) | (level.IsTileAdjacentToWall(x - 1, y - 1) ? 0b1 : 0b0);
            
            _internalMap[i] = adjFloors;
        }
    }

    public AdjacentNode[] GetAdjacentNodes(int index)
    {
        int[] adj = _internalMap[index];
        var result = new AdjacentNode[adj.Length];
        for (int i = 0; i < adj.Length; i++)
        {
            result[i] = new AdjacentNode(adj[i] >> 1, adj[i] & 0b1);
        }
        return result;
    }

    public AdjacentNode[] GetAdjacentNodes(int x, int y) => GetAdjacentNodes((y * _level.Width) + x);
}

public readonly struct AdjacentNode(int index, int cost)
{
    public readonly int Index = index;
    public readonly int Cost = cost;
}