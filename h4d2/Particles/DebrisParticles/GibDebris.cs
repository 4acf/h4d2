using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles;

public class GibDebris : Debris
{
    private readonly int _color;
    
    public GibDebris(Level level, Position position, int color)
        : base(level, position, DebrisConfigs.Gib)
    {
        _color = color;
    }

    public override void Update(double elapsedTime)
    {
        base.Update(elapsedTime);
        var blood = new BloodDebris(_level, _position.Copy());
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