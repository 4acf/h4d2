using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Clouds.Cloudlets;

namespace H4D2.Particles;

public class HealCloud : Particle
{
    private const double _particleMultiplier = 2.0;
    private const double _lifetime = 0.5;
    private const double _radius = 25.0;
    
    private double _timeToLiveSeconds;
    private readonly double _maxLifeSeconds;
    
    public HealCloud(Level level, Position position)
        : base(level, position)
    {
        _timeToLiveSeconds = _lifetime;
        _maxLifeSeconds = _timeToLiveSeconds;
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
            double distance = _radius - ((_timeToLiveSeconds / _maxLifeSeconds) * _radius);
            double randomMult = RandomSingleton.Instance.NextDouble();
            double dx = Math.Cos(randomDirection) * distance * randomMult;
            double dy = Math.Sin(randomDirection) * distance * randomMult;
            double dz = RandomSingleton.Instance.NextDouble() * 2;
            Position translatedPositionCopy = _position.CopyAndTranslate(dx, dy, dz);
            var healParticle = new HealCloudlet(_level, translatedPositionCopy);
            _level.AddParticle(healParticle);
        }
    }
}