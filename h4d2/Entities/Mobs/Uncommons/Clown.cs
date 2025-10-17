using H4D2.Levels;

namespace H4D2.Entities.Mobs.Uncommons;
using Cfg = UncommonConfig;

public class Clown : Uncommon
{
    public Clown(Level level, int xPosition, int yPosition) 
        : base(level, Cfg.Clown, Cfg.ClownHealth, Cfg.ClownSpeed, xPosition, yPosition)
    {
        
    }
}