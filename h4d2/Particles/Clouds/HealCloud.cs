using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Clouds.Cloudlets;

namespace H4D2.Particles.Clouds;

public class HealCloud : Cloud<HealCloudlet>
{
    private const double _healCloudRadius = 25.0;
    
    public HealCloud(Level level, Position position)
        : base(
            level,
            position,
            CloudConfigs.Heal,
            _healCloudRadius,
            (lvl, pos) => new HealCloudlet(lvl, pos)    
        )
    {
        
    }
}