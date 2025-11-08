using H4D2.Entities.Hazards;
using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Particles.DebrisParticles;

public class FuelDebris : Debris
{
    private const double _decay = 0.05;
    private const double _inertia = 0.5;
    private const int _color = 0x4d4c47;
    
    public FuelDebris(Level level, Position position)
        : base(level, position, DebrisConfigs.Fuel)
    {
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
        _despawnTimer.Update(elapsedTime);
        if (_despawnTimer.IsFinished)
        {
            Removed = true;
        }
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
}