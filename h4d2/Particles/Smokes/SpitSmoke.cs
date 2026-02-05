using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.Particles.Smokes;

public class SpitSmoke : Particle
{
    private const double _minLifetime = 0.4;
    private const double _maxLifetime = 0.7;
    private const double _gravity = 0.5;
    private const double _minOpacity = 0.0;
    private const double _maxOpacity = 0.5;
    private const int _color = 0x9fff05;
    
    private readonly CountdownTimer _despawnTimer;
    
    public SpitSmoke(Level level, Position position)
        : base(level, position)
    {
        double randomDouble = RandomSingleton.Instance.NextDouble();
        double lifetime = randomDouble * ((_maxLifetime - _minLifetime) + _minLifetime);
        _despawnTimer = new CountdownTimer(lifetime);
    }

    public override void Update(double elapsedTime)
    {
        _despawnTimer.Update(elapsedTime);
        if (_despawnTimer.IsFinished)
        {
            Removed = true;
            return;
        }
        
        _velocity.Z += _gravity * elapsedTime;
        _Move();
    }
    
    private void _Move()
    {
        int steps = (int)(Math.Sqrt(_velocity.HypotenuseSquared) + 1);
        for (int i = 0; i < steps; i++)
        {
            _position.Z += _velocity.Z / steps;
        }
    }
    
    protected override void Render(H4D2BitmapCanvas screen, int xCorrected, int yCorrected)
    {
        double opacity = 1 - _despawnTimer.Percentage;
        opacity = Math.Clamp(opacity, _minOpacity, _maxOpacity);
        screen.SetPixelBlend(xCorrected, yCorrected, _color, opacity);
    }
}