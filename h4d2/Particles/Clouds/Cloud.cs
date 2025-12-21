using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Clouds.Cloudlets;

namespace H4D2.Particles.Clouds;

public abstract class Cloud<T> : Particle where T : Cloudlet
{
    protected readonly double _particleMultiplier;
    protected readonly double _radius;
    protected readonly CountdownTimer _despawnTimer;
    protected readonly Func<Level, Position, T> _factory;     
    
    protected Cloud(
        Level level, 
        Position position, 
        CloudConfig config, 
        double radius,
        Func<Level, Position, T> factory
    )
        : base(level, position)
    {
        _particleMultiplier = config.ParticleMultiplier;
        _radius = radius;
        _despawnTimer = new CountdownTimer(config.Lifetime);
        _factory = factory;
    }

    public override void Update(double elapsedTime)
    {
        _despawnTimer.Update(elapsedTime);
        if (_despawnTimer.IsFinished)
        {
            Removed = true;
            return;
        }

        double percentageComplete = _despawnTimer.Percentage;
        int newParticles = (int)((_despawnTimer.Percentage) * _particleMultiplier);
        for (int i = 0; i < newParticles; i++)
        {
            double randomDirection = RandomSingleton.Instance.NextDouble() * (2 * Math.PI);
            double distance = _radius - (percentageComplete * _radius);
            double randomMult = RandomSingleton.Instance.NextDouble();
            double dx = Math.Cos(randomDirection) * distance * randomMult;
            double dy = Math.Sin(randomDirection) * distance * randomMult;
            double dz = RandomSingleton.Instance.NextDouble() * 2;
            Position translatedPositionCopy = _position.CopyAndTranslate(dx, dy, dz);
            
            Tile tile = Level.GetTilePosition((translatedPositionCopy.X, translatedPositionCopy.Y));
            if (_level.IsWall(tile))
                continue;
            
            Cloudlet cloudlet = _factory(_level, translatedPositionCopy);
            _level.AddParticle(cloudlet);
        }
    }
}