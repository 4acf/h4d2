using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Smokes;

namespace H4D2.Particles.Clouds.Cloudlets;

public class SmokerSmokeCloudlet : Cloudlet
{
    public SmokerSmokeCloudlet(Level level, Position position)
        : base(level, position, CloudletConfigs.SmokerSmoke)
    {
        
    }

    public override void Update(double elapsedTime)
    {
        Removed = true;
        var smokerSmoke = new SmokerSmoke(_level, _position.Copy(), new ReadonlyVelocity());
        _level.AddParticle(smokerSmoke);
    }
}