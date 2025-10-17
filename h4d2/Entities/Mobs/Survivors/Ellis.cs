using H4D2.Levels;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Ellis : Survivor
{
    public Ellis(Level level, int xPosition, int yPosition) : base(level, Cfg.Ellis, xPosition, yPosition)
    {
        
    }
}