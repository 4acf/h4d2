using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Particles.DebrisParticles.Granules;

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
        var blood = new Blood(_level, _position.Copy(), _velocity.ReadonlyCopy());
        _level.AddParticle(blood);
    }
    
    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        screen.Fill(xCorrected, yCorrected, xCorrected + 1, yCorrected + 1, _color);
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.Fill(xCorrected, yCorrected, xCorrected + 1, yCorrected + 1);
    }
}