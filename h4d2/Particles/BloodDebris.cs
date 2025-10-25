using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;

public class BloodDebris : Debris
{
    private const double _decay = 0.05;
    private const double _inertia = 0.5;
    
    public BloodDebris(Level level, double xPosition, double yPosition, double zPosition)
        : base(level, xPosition, yPosition, zPosition, 0.96, 0.1)
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
        screen.SetPixel(xCorrected, yCorrected, 0xa00000);
    }

    protected override void RenderShadow(Bitmap screen, int xCorrected, int yCorrected)
    {
        screen.SetPixelBlend(xCorrected, yCorrected, 0x0, 0.9);
    }
}