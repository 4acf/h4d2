using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles;

public class BloodDebris : Debris
{
    private const double _decay = 0.05;
    private const double _inertia = 0.5;
    private const int _color = 0xa00000;
    
    public BloodDebris(Level level, Position position)
        : base(level, position, DebrisConfigs.Blood)
    {
        
    }

    public void DampVelocities(double parentXVelocity, double parentYVelocity, double parentZVelocity)
    {
        _xVelocity *= _decay;
        _yVelocity *= _decay;
        _zVelocity *= _decay;
        _xVelocity += parentXVelocity * _inertia;
        _yVelocity += parentYVelocity * _inertia;
        _zVelocity += parentZVelocity * _inertia;
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