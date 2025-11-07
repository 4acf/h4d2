using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles;

public class BloodDebris : Debris
{
    private const double _decay = 0.05;
    private const double _inertia = 0.5;
    private const double _bloodDrag = 0.96;
    private const double _bloodBounce = 0.1;
    private const int _color = 0xa00000;
    
    public BloodDebris(Level level, Position position)
        : base(level, position, _bloodDrag, _bloodBounce)
    {
        
    }

    public void DampVelocities(double elapsedTime, double parentXVelocity, double parentYVelocity, double parentZVelocity)
    {
        double deltaDecay = Math.Pow(_decay, _baseFramerate * elapsedTime);
        double deltaInertia = _inertia * (_baseFramerate * elapsedTime);
        _xVelocity *= deltaDecay;
        _yVelocity *= deltaDecay;
        _zVelocity *= deltaDecay;
        _xVelocity += parentXVelocity * deltaInertia;
        _yVelocity += parentYVelocity * deltaInertia;
        _zVelocity += parentZVelocity * deltaInertia;
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