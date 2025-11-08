using H4D2.Infrastructure;
using H4D2.Levels;
using H4D2.Particles.Smokes;

namespace H4D2.Particles;

public class ExplosionFlame : Particle
{
    private const double _frameDuration = 1.0 / 16.0;
    
    private int _frame;
    private double _timeSinceLastFrameUpdate;
    
    public ExplosionFlame(Level level, Position position)
        : base(level, position)
    {
        _frame = 0;
    }

    public override void Update(double elapsedTime)
    {
        _timeSinceLastFrameUpdate += elapsedTime;
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            _frame += 1;
            _timeSinceLastFrameUpdate -= _frameDuration;
        }

        if (_frame >= Art.Explosion.Length)
        {
            Removed = true;
            var smoke = new Smoke(_level, _position.Copy(), 0, 0);
            _level.AddParticle(smoke);
        }
    }

    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        Bitmap bitmap = Art.Explosion[_frame];
        int radius = Art.ParticleSize / 2;
        screen.Draw(bitmap, xCorrected - radius, yCorrected + radius);
    }
}