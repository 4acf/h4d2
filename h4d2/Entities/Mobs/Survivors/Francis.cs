using H4D2.Levels;
using H4D2.Weapons;

namespace H4D2.Entities.Mobs.Survivors;
using Cfg = SurvivorConfig;

public class Francis : Survivor
{
    public Francis(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Francis, xPosition, yPosition)
    {
        _weapon = new AutoShotgun(level, this);
    }    
}