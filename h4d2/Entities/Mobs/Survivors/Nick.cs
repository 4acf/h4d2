using H4D2.Levels;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Nick : Survivor
{
    public Nick(Level level, int xPosition, int yPosition) : base(level, Cfg.Nick, xPosition, yPosition)
    {
        
    }    
}