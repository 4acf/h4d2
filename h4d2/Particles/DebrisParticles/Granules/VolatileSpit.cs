using H4D2.Entities.Hazards;
using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Smokes;

namespace H4D2.Particles.DebrisParticles.Granules;

public class VolatileSpit : Granule
{
    private const double _minSmokeDelay = 0.3;
    private const double _maxSmokeDelay = 0.7;
    
    private readonly CountdownTimer _smokeTimer;
    
    public VolatileSpit(Level level, Position position, ReadonlyVelocity parentVelocity)
        : base(level, position, GranuleConfigs.VolatileSpit, parentVelocity)
    {
        double randomDouble = RandomSingleton.Instance.NextDouble();
        double smokeDelay =  randomDouble * ((_maxSmokeDelay - _minSmokeDelay) + _minSmokeDelay);
        _smokeTimer = new CountdownTimer(smokeDelay);
        
        var spitPuddle = new SpitPuddle(_level, _position.Copy());
        _level.AddHazard(spitPuddle);
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        _smokeTimer.Update(elapsedTime);
        if (_smokeTimer.IsFinished)
        {
            if (RandomSingleton.Instance.Next(500) != 0)
                return;
            var spitSmoke = new SpitSmoke(_level, _position.Copy());
            _level.AddParticle(spitSmoke);
            _smokeTimer.Reset();
        }
    }
}