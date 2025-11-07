using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;
using Cfg = ParticleConfig;

public class BileDebris : Debris
{
    private const double _decay = 0.05;
    private const double _inertia = 0.5;
    private const double _bileDrag = 0.96;
    private const double _bileBounce = 0.1;
    private new const double _maxLifetime = 20.0;
    private const int _color = 0x5a6e38;
    
    public BileDebris(Level level, Position position)
        : base(level, position, _bileDrag, _bileBounce, _maxLifetime)
    {
        
    }

    public void DampVelocities(double elapsedTime, double parentXVelocity, double parentYVelocity, double parentZVelocity)
    {
        double deltaDecay = Math.Pow(_decay, Cfg.BaseFramerate * elapsedTime);
        double deltaInertia = _inertia * (Cfg.BaseFramerate * elapsedTime);
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