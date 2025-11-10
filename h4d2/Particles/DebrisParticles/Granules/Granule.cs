using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles.Granules;

public abstract class Granule : Debris
{
    private const double _decay = 0.05;
    private const double _inertia = 0.5;
    
    private readonly int _color;
    
    protected Granule(Level level, Position position, GranuleConfig config, ReadonlyVelocity parentVelocity)
        : base(level, position, config)
    {
        _color = config.Color;
        _velocity.X *= _decay;
        _velocity.Y *= _decay;
        _velocity.Z *= _decay;
        _velocity.X += parentVelocity.X * _inertia;
        _velocity.Y += parentVelocity.Y * _inertia;
        _velocity.Z += parentVelocity.Z * _inertia;
    }
    
    protected override void Render(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.SetPixel(xCorrected, yCorrected, _color);
    }

    protected override void RenderShadow(ShadowBitmap shadows, int xCorrected, int yCorrected)
    {
        shadows.SetPixel(xCorrected, yCorrected);
    }
}