using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using Cfg = UncommonConfig;

public class Worker : Uncommon
{
    public Worker(Level level, Position position) 
        : base(
            level,
            position,
            Cfg.Worker,
            Cfg.WorkerHealth,
            Cfg.WorkerSpeed,
            Cfg.Damage,
            Cfg.WorkerColor
        )
    {
        
    }    
}