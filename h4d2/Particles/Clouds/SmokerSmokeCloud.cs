using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Clouds.Cloudlets;

namespace H4D2.Particles.Clouds;

public class SmokerSmokeCloud : Cloud<SmokerSmokeCloudlet>
{
    public SmokerSmokeCloud(Level level, Position position, double splashRadius)
        : base(
            level,
            position,
            CloudConfigs.SmokerSmoke,
            splashRadius,
            (lvl, pos) => new SmokerSmokeCloudlet(lvl, pos)
        )
    {
        
    }
}