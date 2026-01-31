using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Zombies.Uncommons;

public class JimmyGibbs : Uncommon
{
    public JimmyGibbs(Level level, Position position)
        : base(level, position, UncommonConfigs.JimmyGibbs)
    {
        
    }
}