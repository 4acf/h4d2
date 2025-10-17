using H4D2.Levels;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Bill : Survivor
{
    public Bill(Level level, int xPosition, int yPosition) : base(level,Cfg.Bill,  xPosition, yPosition)
    {
        
    }
}