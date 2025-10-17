using H4D2.Levels;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Zoey : Survivor
{
    public Zoey(Level level, int xPosition, int yPosition) : base(level, Cfg.Zoey, xPosition, yPosition)
    {
        
    }
}