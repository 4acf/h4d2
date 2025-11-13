using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Hazards;

public class SpitPuddle : Hazard
{
    public SpitPuddle(Level level, Position position)
        : base(level, position, HazardConfigs.Spit)
    {
        
    }
}