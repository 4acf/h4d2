using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;
using Cfg = ParticleConfig;

public class DeathSplatterDebris : Debris
{
    private readonly int _color;
    
    public DeathSplatterDebris(Level level, double xPosition, double yPosition, double zPosition, int color)
        : base(level, xPosition, yPosition, zPosition, Cfg.DeathSplatterDrag, Cfg.DeathSplatterBounce)
    {
        _color = color;
        _timeToLiveSeconds *= 1.5;
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        var blood = new BloodDebris(_level, XPosition, YPosition, ZPosition);
        blood.DampVelocities(elapsedTime, _xVelocity, _yVelocity, _zVelocity);
        _level.AddParticle(blood);
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.Fill(xCorrected, yCorrected, xCorrected + 1, yCorrected + 1, _color);
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.BlendFill(xCorrected, yCorrected, xCorrected + 1, yCorrected + 1, Art.ShadowColor, Art.ShadowBlend);
    }
}