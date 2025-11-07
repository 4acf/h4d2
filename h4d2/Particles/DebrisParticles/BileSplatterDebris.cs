using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles;

public class BileSplatterDebris : Debris
{
    private const double _bileSplatterDrag = 0.95;
    private const double _bileSplatterBounce = 0.6;

    public BileSplatterDebris(Level level, Position position)
        : base(level, position, _bileSplatterDrag, _bileSplatterBounce)
    {
        
    }
    
    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        if (RandomSingleton.Instance.Next(2) != 0)
            return;
        var bile = new BileDebris(_level, _position.Copy());
        bile.DampVelocities(elapsedTime, _xVelocity, _yVelocity, _zVelocity);
        _level.AddParticle(bile);
    }
}