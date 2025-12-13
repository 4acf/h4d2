using H4D2.Infrastructure;

namespace H4D2.Levels.Pathfinders;

public class Pathfinder
{
    private readonly Level _level;
    
    public Pathfinder(Level level)
    {
        _level = level;
    }

    public double CorrectDirectionToAvoidWalls(ReadonlyPosition position, double direction)
    {
        double result = direction;
        var (currentTileX, currentTileY) = _level.GetTilePosition(position);

        bool wallToN = _level.IsWall(currentTileX, currentTileY - 1);
        bool wallToE = _level.IsWall(currentTileX + 1, currentTileY);
        bool wallToS = _level.IsWall(currentTileX, currentTileY + 1);
        bool wallToW = _level.IsWall(currentTileX - 1, currentTileY);
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
        return 0.0;
    }
}