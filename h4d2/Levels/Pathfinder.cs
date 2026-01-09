using H4D2.Entities;
using H4D2.Entities.Mobs.Survivors;
using H4D2.Entities.Mobs.Zombies.Commons;
using H4D2.Infrastructure;

namespace H4D2.Levels;

public class Pathfinder
{
    private readonly Level _level;
    private readonly Entity _entity;
    private Path? _path;
    
    public Pathfinder(Level level, Entity entity)
    {
        _level = level;
        _entity = entity;
        _path = null;
    }

    public void InvalidatePath()
    {
        _path = null;
    }
    
    public double CorrectDirectionToAvoidWalls(ReadonlyPosition position, double direction)
    {
        double result = direction;
        Tile currentTile = Level.GetTilePosition(position);

        bool wallToN = _level.IsWall(currentTile.X, currentTile.Y - 1);
        bool wallToE = _level.IsWall(currentTile.X + 1, currentTile.Y);
        bool wallToS = _level.IsWall(currentTile.X, currentTile.Y + 1);
        bool wallToW = _level.IsWall(currentTile.X - 1, currentTile.Y);
        bool directionPointingN = result < Math.PI;
        bool directionPointingE = (3 * Math.PI / 2) < result || result < (Math.PI / 2);
        bool directionPointingS = result > Math.PI;
        bool directionPointingW = (Math.PI / 2) < result && result < (3 * Math.PI / 2);

        if (wallToN && directionPointingN)
            result = SwapNandS(result);
        if (wallToE && directionPointingE)
            result = SwapWandE(result);
        if (wallToS && directionPointingS)
            result = SwapNandS(result);
        if (wallToW && directionPointingW)
            result = SwapWandE(result);
        
        return result;
        
        double SwapWandE(double dir) => Math.Atan2(Math.Sin(dir), Math.Cos(dir) * -1);
        double SwapNandS(double dir) => Math.Atan2(Math.Sin(dir) * -1, Math.Cos(dir));
    }
    
    public double GetNextDirection(ReadonlyPosition start, ReadonlyPosition end)
    {
        _path ??= new Path(_level, start, end);
        
        bool targetMovedTiles = Level.GetTilePosition(_path.End) != Level.GetTilePosition(end);
        if (targetMovedTiles)
        {
            bool targetMovedTooFar = !_level.HasLineOfSight(_path.End, end);
            if(targetMovedTooFar)
                _path = new Path(_level, start, end);
        }

        Tile startTile = Level.GetTilePosition(start);
        if(!_path.Contains(startTile))
            _path = new Path(_level, start, end);
        
        return _path.GetNextDirection(startTile);
    }

    public bool HasLineOfSight(Entity target)
        => _level.HasLineOfSight(_entity.NWPosition, target.NWPosition) && 
           _level.HasLineOfSight(_entity.NEPosition, target.NEPosition) && 
           _level.HasLineOfSight(_entity.SWPosition, target.SWPosition) && 
           _level.HasLineOfSight(_entity.SEPosition, target.SEPosition);

    private class Path
    {
        private readonly Queue<Tile> _path;
        public readonly ReadonlyPosition End;

        public Path(Level level, ReadonlyPosition start, ReadonlyPosition end)
        {
            _path = new Queue<Tile>();
            End = end;
            
            Tile startTile = Level.GetTilePosition(start);
            Tile endTile = Level.GetTilePosition(end);
            
            var pq = new UpdatingPQ();
            var cameFrom = new Dictionary<Tile, Tile>();
            var gScores = new Dictionary<Tile, double>
            {
                [startTile] = 0
            };
            pq.Enqueue(startTile, gScores[startTile] + _TileDistance(startTile, endTile));

            while (pq.Count > 0)
            {
                Tile currentTile = pq.Dequeue();
                if (currentTile == endTile)
                {
                    var stk = new Stack<Tile>();
                    stk.Push(currentTile);
                    while (cameFrom.ContainsKey(currentTile))
                    {
                        currentTile = cameFrom[currentTile];
                        stk.Push(currentTile);
                    }
                    
                    while (stk.Count > 0)
                    {
                        _path.Enqueue(stk.Pop());
                    }
                    return;
                }

                int index = level.TileIndex(currentTile);
                AdjacentNode[] adjNodes = level.CostMap.GetAdjacentNodes(index);
                foreach (var adjNode in adjNodes)
                {
                    Tile adjTile = level.GetTileFromIndex(adjNode.Index);
                    double score = gScores[currentTile] + adjNode.Cost;
                    double adjNodeScore = gScores.GetValueOrDefault(adjTile, double.MaxValue);

                    if (score >= adjNodeScore)
                        continue;
                    
                    cameFrom[adjTile] = currentTile;
                    gScores[adjTile] = score;
                    pq.Enqueue(adjTile, gScores[adjTile] + _TileDistance(adjTile, endTile));
                }
            }
        }
        
        public bool Contains(Tile tile) => _path.Contains(tile);

        public double GetNextDirection(Tile currentTile)
        {
            if (_path.Count == 0)
                return 0.0;
            
            Tile nextTile = _path.Peek(); 
            
            while (_path.Count > 0 && nextTile != currentTile)
            {
                nextTile = _path.Dequeue();
            }

            if (_path.Count <= 1)
                return 0.0;

            _path.Dequeue();
            nextTile = _path.Peek();
            double direction = Math.Atan2(currentTile.Y - nextTile.Y, nextTile.X - currentTile.X);
            return MathHelpers.NormalizeRadians(direction);
        }

        private static double _TileDistance(Tile t1, Tile t2)
        {
            // this works for now but is not proper Chebyshev distance
            // possibly an issue with my A* implementation
            double dx = t1.X - t2.X;
            double dy = t1.Y - t2.Y;
            return Math.Min(Math.Abs(dx), Math.Abs(dy));
        }
        
        private class UpdatingPQ
        {
            public int Count => _currentlyInQueue.Count;
            private readonly PriorityQueue<Tile, double> _pq = new();
            private readonly Dictionary<Tile, double> _currentlyInQueue = [];

            public void Enqueue(Tile tile, double cost)
            {
                if (!_currentlyInQueue.TryGetValue(tile, out double value) || cost < value)
                {
                    _pq.Enqueue(tile, cost);
                    _currentlyInQueue[tile] = cost;
                }
            }

            public Tile Dequeue()
            {
                const double tolerance = 0.00001;
                while (_pq.TryDequeue(out Tile tile, out double cost))
                {
                    if (!_currentlyInQueue.ContainsKey(tile))
                        continue;
                    
                    if (Math.Abs(_currentlyInQueue[tile] - cost) <= tolerance)
                    {
                        _currentlyInQueue.Remove(tile);
                        return tile;
                    }
                }
                throw new InvalidOperationException("UpdatingPQ is empty.");
            }
        }
    }
    
}