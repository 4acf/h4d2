using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.Clouds.Cloudlets;

public abstract class Cloudlet : Particle
{
    protected int _frame;
    protected readonly CountdownTimer _frameUpdateTimer;
    protected readonly Bitmap[] _bitmaps;

    protected Cloudlet(Level level, Position position, CloudletConfig config)
        : base(level, position)
    {
        _frame = 0;
        _frameUpdateTimer = new CountdownTimer(config.FrameDuration);
        _bitmaps = config.Bitmaps;
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
        }
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = _bitmaps[_frame];
        const int radius = Art.ParticleSize / 2;
        screen.Draw(bitmap, xCorrected - radius, yCorrected + radius);
    }
}