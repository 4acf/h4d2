using H4D2.Infrastructure;

namespace H4D2.GUI.GUIParticles;

public class ConfettiGranule
{
    private const double _gravity = 150.0;
    private const double _minLifetime = 1.5;
    private const double _maxLifetime = 2.0;

    public bool Removed { get; private set; }
    private double _x;
    private double _y;
    private readonly double _xVelocity;
    private double _yVelocity;
    private readonly int _color;
    private readonly CountdownTimer _despawnTimer;
    
    public ConfettiGranule(double x, double y, double xVelocity, double yVelocity)
    {
        Removed = false;
        
        _x = x;
        _y = y;
        _xVelocity = xVelocity;
        _yVelocity = yVelocity;

        int r = RandomSingleton.Instance.Next(0xff);
        int g = RandomSingleton.Instance.Next(0xff);
        int b = RandomSingleton.Instance.Next(0xff);
        int color = r;
        color = (color << 8) | g;
        color = (color << 8) | b;
        _color = color;

        double randomDouble = RandomSingleton.Instance.NextDouble();
        _despawnTimer = new CountdownTimer(randomDouble * (_maxLifetime - _minLifetime) + _minLifetime);
    }

    public void Update(double elapsedTime)
    {
        _despawnTimer.Update(elapsedTime);
        if (_despawnTimer.IsFinished)
        {
            Removed = true;
            return;
        }
        
        _yVelocity -= _gravity * elapsedTime;
        _x += _xVelocity * elapsedTime;
        _y += _yVelocity * elapsedTime;
    }

    public void Render(Bitmap screen)
    {
        screen.SetPixelAbsolute((int)_x, (int)_y, _color);
    }
}