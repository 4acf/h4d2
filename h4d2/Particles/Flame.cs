using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;
using H4D2.Particles.Smokes;

namespace H4D2.Particles;

public class Flame : Particle
{
    private const double _frameDuration = 1.0 / 8.0;
    private const double _lifetime = 15.0;
    
    private int _frame;
    private readonly CountdownTimer _frameUpdateTimer;
    private readonly CountdownTimer _despawnTimer;

    public readonly ReadonlyPosition FootPosition;
    
    public Flame(Level level, Position position)
        : base(level, position)
    {
        _frame = 0;
        _frameUpdateTimer = new CountdownTimer(_frameDuration);
        _despawnTimer = new CountdownTimer(_lifetime);

        (double, double) offsets = ScreenSpaceToWorldSpace(
            H4D2Art.ParticleSize / 2.0,
            -H4D2Art.ParticleSize
        );
        FootPosition = new ReadonlyPosition(position.X + offsets.Item1, position.Y + offsets.Item2, 0);
    }

    public override void Update(double elapsedTime)
    {
        _despawnTimer.Update(elapsedTime);
        if (_despawnTimer.IsFinished)
        {
            if (Probability.OneIn(2))
            {
                var smoke = new Smoke(_level, _position.Copy(), new ReadonlyVelocity());
                _level.AddParticle(smoke);
            }
            Removed = true;
        }
        
        _frameUpdateTimer.Update(elapsedTime);
        while (_frameUpdateTimer.IsFinished)
        {
            _frame = RandomSingleton.Instance.Next(4);
            _frameUpdateTimer.AddDuration();
        }
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = H4D2Art.Particles.Fire[_frame];
        screen.Draw(bitmap, xCorrected, yCorrected);
    }
}