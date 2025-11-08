using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.Clouds.Cloudlets;

public class HealCloudlet : Cloudlet
{
    public HealCloudlet(Level level, Position position)
        : base(level, position, CloudletConfigs.Heal)
    {
        _frame = 0;
    }
}