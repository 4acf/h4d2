using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;

public class Flame : Particle
{
    private const double _frameDuration = 1.0 / 8.0;
    private const double _maxLifetime = 15.0;
    
    private int _frame;
    private double _timeSinceLastFrameUpdate;
    private double _timeToLiveSeconds;
    
    public Flame(Level level, Position position)
        : base(level, position)
    {
        _frame = 0;
        _timeSinceLastFrameUpdate = 0.0;
        _timeToLiveSeconds = _maxLifetime;
    }

    public override void Update(double elapsedTime)
    {
        _timeToLiveSeconds -= elapsedTime;
        if (_timeToLiveSeconds <= 0)
        {
            var smoke = new Smoke(_level, _position.Copy(), 0, 0);
            _level.AddParticle(smoke);
            Removed = true;
        }
        
        _timeSinceLastFrameUpdate += elapsedTime;
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            _frame = RandomSingleton.Instance.Next(4);
            _timeSinceLastFrameUpdate -= _frameDuration;
        }
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = Art.Fire[_frame];
        screen.Draw(bitmap, xCorrected, yCorrected);
    }
}