using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Smokes;

namespace H4D2.Particles.Clouds.Cloudlets;

public class ExplosionFlame : Cloudlet
{
    public ExplosionFlame(Level level, Position position)
        : base(level, position, CloudletConfigs.ExplosionFlame)
    {
        
    }

    public override void Update(double elapsedTime)
    {
        _frameUpdateTimer.Update(elapsedTime);
        while (_frameUpdateTimer.IsFinished)
        {
            _frame += 1;
            _frameUpdateTimer.AddDuration();
        }

        if (_frame >= _bitmaps.Length)
        {
            Removed = true;
            var smoke = new Smoke(_level, _position.Copy(), new ReadonlyVelocity());
            _level.AddParticle(smoke);
        }
    }
}