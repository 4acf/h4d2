using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;
using Cfg = ParticleConfig;

public class BloodSplatterDebris : Debris
{
    private const double _bloodSplatterDrag = 0.98;
    private const double _bloodSplatterBounce = 0.6;
    private const double _lifetimeScale = 0.25;
    
    public BloodSplatterDebris(Level level, Position position)
        : base(level, position, _bloodSplatterDrag, _bloodSplatterBounce)
    {
        _timeToLiveSeconds *= _lifetimeScale;
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        if (RandomSingleton.Instance.Next(2) != 0)
            return;
        var blood = new BloodDebris(_level, _position.Copy());
        blood.DampVelocities(elapsedTime, _xVelocity, _yVelocity, _zVelocity);
        _level.AddParticle(blood);
    }
}