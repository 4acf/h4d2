using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles;

public class BloodSplatterDebris : Debris
{
    public BloodSplatterDebris(Level level, Position position)
        : base(level, position, DebrisConfigs.BloodSplatter)
    {
        
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        if (RandomSingleton.Instance.Next(2) != 0)
            return;
        var blood = new BloodDebris(_level, _position.Copy());
        blood.DampVelocities(_xVelocity, _yVelocity, _zVelocity);
        _level.AddParticle(blood);
    }
}