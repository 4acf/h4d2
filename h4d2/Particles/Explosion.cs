using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;
using Cfg = ParticleConfig;

public class Explosion : Particle
{
    private const double _particleMultiplier = 10;
    
    private double _timeToLiveSeconds;
    private readonly double _maxLifeSeconds;
    private readonly double _splashRadius;
    
    public Explosion(Level level, double xPosition, double yPosition, double zPosition, double splashRadius)
        : base(level, xPosition, yPosition, zPosition)
    {
        _timeToLiveSeconds = Cfg.ExplosionLifetime;
        _maxLifeSeconds = _timeToLiveSeconds;
        _splashRadius = splashRadius;
    }

    public override void Update(double elapsedTime)
    {
        _timeToLiveSeconds -= elapsedTime;
        if (_timeToLiveSeconds <= 0)
        {
            Removed = true;
            return;
        }

        int newParticles = (int)((_timeToLiveSeconds / _maxLifeSeconds) * _particleMultiplier);
        for (int i = 0; i < newParticles; i++)
        {
            double randomDirection = RandomSingleton.Instance.NextDouble() * (2 * Math.PI);
            double distance = _splashRadius - ((_timeToLiveSeconds / _maxLifeSeconds) * _splashRadius);
            double randomMult = RandomSingleton.Instance.NextDouble();
            double dx = Math.Cos(randomDirection) * distance * randomMult;
            double dy = Math.Sin(randomDirection) * distance * randomMult;
            double dz = RandomSingleton.Instance.NextDouble() * 2;
            var flame = new Flame(_level, XPosition + dx, YPosition + dy, ZPosition + dz);
            _level.AddParticle(flame);
        }
    }
    
}