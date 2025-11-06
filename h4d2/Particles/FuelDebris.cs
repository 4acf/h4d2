using H4D2.Entities.Hazards;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles;
using Cfg = ParticleConfig;

public class FuelDebris : Debris
{
    private const double _decay = 0.05;
    private const double _inertia = 0.5;
    private const double _fuelDrag = 0.98;
    private const double _fuelBounce = 0;
    private const double _maxLifetime = 20.0;
    private const int _color = 0x4d4c47;
    
    public FuelDebris(Level level, Position position)
        : base(level, position, _fuelDrag, _fuelBounce)
    {
        _timeToLiveSeconds = _maxLifetime;

        if (RandomSingleton.Instance.Next(7) == 0)
        {
            var fire = new Fire(_level, _position.CopyAndTranslate(
                -Art.ParticleSize / 2.0,
                Art.ParticleSize,
                0)
            );
            _level.AddHazard(fire);
        }
    }

    public override void Update(double elapsedTime)
    {
        _timeToLiveSeconds -= elapsedTime;
        if (_timeToLiveSeconds <= 0)
        {
            Removed = true;
        }
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
}