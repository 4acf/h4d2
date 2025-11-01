using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Specials;

public class Hunter : Special
{
    public Hunter(Level level, Position position) 
        : base(level, position, SpecialConfigs.Hunter)
    {
        
    }    
}