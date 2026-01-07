using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI.GUIParticles;

public class ConfettiEmitter
{
    private const int _numConfettiParticles = 30;
    private const int _velocityFactor = 150;
    public bool Removed { get; private set; }
    private readonly ConfettiGranule[] _granules;
    
    public ConfettiEmitter(int x, int y)
    {
        Removed = false;
        _granules = new ConfettiGranule[_numConfettiParticles];
        
        for (int i = 0; i < _numConfettiParticles; i++)
        {
            double randomXVelocity =
                (RandomSingleton.Instance.NextDouble() * _velocityFactor) - (_velocityFactor / 2.0);
            double randomYVelocity =
                RandomSingleton.Instance.NextDouble() * _velocityFactor;
            _granules[i] = new ConfettiGranule(x, y, randomXVelocity, randomYVelocity);
        }
    }

    public void Update(double elapsedTime)
    {
        int removed = 0;
        for (int i = 0; i < _numConfettiParticles; i++)
        {
            _granules[i].Update(elapsedTime);
            if (_granules[i].Removed)
                removed++;
        }

        if (removed == _numConfettiParticles)
            Removed = true;
    }

    public void Render(H4D2BitmapCanvas screen)
    {
        for (int i = 0; i < _numConfettiParticles; i++)
        {
            if(!_granules[i].Removed)
                _granules[i].Render(screen);
        }
    }
}