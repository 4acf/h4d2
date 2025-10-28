using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;

public class BloodSplatterDebris : Debris
{
    public BloodSplatterDebris(Level level, double xPosition, double yPosition, double zPosition)
        : base(level, xPosition, yPosition, zPosition, 0.98, 0.6)
    {
        _timeToLiveSeconds /= 4;
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        if (RandomSingleton.Instance.Next(2) != 0)
            return;
        var blood = new BloodDebris(_level, XPosition, YPosition, ZPosition);
        blood.DampVelocities(elapsedTime, _xVelocity, _yVelocity, _zVelocity);
        _level.AddParticle(blood);
    }
}