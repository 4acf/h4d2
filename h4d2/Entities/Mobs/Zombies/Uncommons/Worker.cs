using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using Cfg = UncommonConfig;

public class Worker : Uncommon
{
    public Worker(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Worker, Cfg.WorkerHealth, Cfg.WorkerSpeed, xPosition, yPosition, Cfg.WorkerColor)
    {
        
    }    
}