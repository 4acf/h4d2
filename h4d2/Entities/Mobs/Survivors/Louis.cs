using H4D2.Levels;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;
public class Louis : Survivor
{
    public Louis(Level level, int xPosition, int yPosition) : base(level, Cfg.Louis, xPosition, yPosition)
    {
        
    }    
}