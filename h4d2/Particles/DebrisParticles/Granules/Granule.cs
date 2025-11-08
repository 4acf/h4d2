using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles.Granules;

public abstract class Granule : Debris
{
    private const double _decay = 0.05;
    private const double _inertia = 0.5;
    
    private readonly int _color;
    
    protected Granule(Level level, Position position, GranuleConfig config, double xv, double yv, double zv)
        : base(level, position, config)
    {
        _color = config.Color;
        _xVelocity *= _decay;
        _yVelocity *= _decay;
        _zVelocity *= _decay;
        _xVelocity += xv * _inertia;
        _yVelocity += yv * _inertia;
        _zVelocity += zv * _inertia;
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.SetPixel(xCorrected, yCorrected, _color);
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.SetPixelBlend(xCorrected, yCorrected, Art.ShadowColor, Art.ShadowBlend);
    }
}