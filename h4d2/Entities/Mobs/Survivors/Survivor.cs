using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Survivors;

public class Survivor : Mob
{
    private readonly int _character;
    private int _walkStep;
    private int _lastNonZeroWalkStep;
    private const double _frameDuration = 1.0 / 8.0;
    private double _timeSinceLastFrameUpdate;
    
    protected Survivor(Level level, int character, int xPosition, int yPosition) : base(level, 100, 220, xPosition, yPosition)
    {
        _character = character;
        _walkStep = 0;
        _lastNonZeroWalkStep = 0;
        _timeSinceLastFrameUpdate = 0.0;
    }

    public override void Update(double elapsedTime)
    {
        double frameFactor = elapsedTime * 60;
        
        // angular velocity calculations are still a work in progress, they don't yet change based on framerate
        _xVelocity *= 0.5;
        _yVelocity *= 0.5;
        _angularVelocity *= 0.9;
        _angularVelocity += ((RandomSingleton.Instance.NextDouble() - RandomSingleton.Instance.NextDouble()) * RandomSingleton.Instance.NextDouble()) * 0.1;
        _directionRadians += _angularVelocity;
        
        double moveSpeed = (0.2 * _speed / 220) * frameFactor;
        _xVelocity += Math.Cos(_directionRadians) * moveSpeed;
        _yVelocity += Math.Sin(_directionRadians) * moveSpeed;
        _AttemptMove();

        if (_xVelocity == 0 || _yVelocity == 0)
        {
            _angularVelocity += ((RandomSingleton.Instance.NextDouble() -RandomSingleton.Instance.NextDouble()) * RandomSingleton.Instance.NextDouble()) * 0.4;
        }
        
        _timeSinceLastFrameUpdate += elapsedTime;

        // to be refactored eventually
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            if (_walkStep == 0)
            {
                if (_lastNonZeroWalkStep == 0 || _lastNonZeroWalkStep == 2)
                {
                    _walkStep = 1;
                    _lastNonZeroWalkStep = 1;
                }
                else
                {
                    _walkStep = 2;
                    _lastNonZeroWalkStep = 2;
                }
            }
            else
            {
                _walkStep = 0;
            }
            _timeSinceLastFrameUpdate -= _frameDuration;
        }
    }

    public override void Render(Bitmap screen)
    {
        Bitmap animationCycleBitmap = Art.Survivors[_character][_walkStep];
        screen.Draw(animationCycleBitmap, (int)XPosition, (int)YPosition);
    }
}