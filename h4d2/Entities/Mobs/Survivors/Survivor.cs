using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2.Entities.Mobs.Survivors;

public class Survivor : Mob
{
    private readonly int _character;
    private int _walkStep;
    private int _walkFrame;
    private const double _frameDuration = 1.0 / 8.0;
    private double _timeSinceLastFrameUpdate;
    
    protected Survivor(Level level, int character, int xPosition, int yPosition) 
        : base(level, 100, 220, xPosition, yPosition)
    {
        _character = character;
        _walkStep = 0;
        _walkFrame = 0;
        _timeSinceLastFrameUpdate = 0.0;
    }

    public override void Update(double elapsedTime)
    {
        _UpdatePosition(elapsedTime);
        _UpdateSprite(elapsedTime);
    }

    private void _UpdatePosition(double elapsedTime)
    {
        Random random = RandomSingleton.Instance;
        double frameFactor = elapsedTime * 60;
        
        // angular velocity calculations are still a work in progress, they don't yet change based on framerate
        _xVelocity *= 0.5;
        _yVelocity *= 0.5;
        _angularVelocity *= 0.9;
        _angularVelocity += ((random.NextDouble() - random.NextDouble()) * random.NextDouble()) * 0.1;
        _directionRadians += _angularVelocity;
        while (_directionRadians < 0) _directionRadians += 2 * Math.PI;
        
        double moveSpeed = (0.2 * _speed / 220) * frameFactor;
        _xVelocity += Math.Cos(_directionRadians) * moveSpeed;
        _yVelocity += Math.Sin(_directionRadians) * moveSpeed;
        _AttemptMove();

        if (_xVelocity == 0 || _yVelocity == 0)
        {
            _angularVelocity += ((random.NextDouble() - random.NextDouble()) * random.NextDouble()) * 0.4;
        }
    }
    
    private void _UpdateSprite(double elapsedTime)
    {
        _timeSinceLastFrameUpdate += elapsedTime;

        int direction = 0;
        int degrees = MathHelpers.RadiansToDegrees(_directionRadians);
        switch (degrees)
        {
            case >= 315:
            case < 45:
                direction = 1;
                _xFlip = false;
                break;
            case < 135:
                direction = 2;
                _xFlip = false;
                break;
            case < 225:
                direction = 1;
                _xFlip = true;
                break;
            default:
                direction = 0;
                _xFlip = false;
                break;
        }
        
        while (_timeSinceLastFrameUpdate >= _frameDuration)
        {
            _walkStep = (_walkStep + 1) % 4;
            int nextFrame = 0;
            if (direction == 1)
            {
                nextFrame = _walkStep switch
                {
                    0 => 3,
                    1 or 3 => 4,
                    2 => 5,
                    _ => nextFrame
                };
            }
            else
            {
                nextFrame = _walkStep switch
                {
                    0 or 2 => 0 + (3 * direction),
                    1 => 1 + (3 * direction),
                    3 => 2 + (3 * direction),
                    _ => nextFrame
                };
            }
            _walkFrame = nextFrame;
            _timeSinceLastFrameUpdate -= _frameDuration;
        }
    }
    
    public override void Render(Bitmap screen)
    {
        Bitmap animationCycleBitmap = Art.Survivors[_character][_walkFrame];
        screen.Draw(animationCycleBitmap, (int)XPosition, (int)YPosition, _xFlip);
    }

    public override void RenderShadow(Bitmap screen)
    {
        int x = (int)XPosition;
        int y = (int)YPosition;
        screen.BlendFill(
            x + Art.SpriteSize - 10,
            y - Art.SpriteSize - 1,
            x + Art.SpriteSize - 7,
            y - Art.SpriteSize - 1,
            0x0,
            0.9            
        );
    }
    
}