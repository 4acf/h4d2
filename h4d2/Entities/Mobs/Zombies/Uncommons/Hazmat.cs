using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;
using Cfg = UncommonConfig;

public class Hazmat : Uncommon
{
    public Hazmat(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Hazmat, Cfg.HazmatHealth, Cfg.HazmatSpeed, xPosition, yPosition)
    {
        
    }
}