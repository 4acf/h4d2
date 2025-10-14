using H4D2.Infrastructure;

namespace H4D2.Entities.Mobs.Uncommons;

public class Uncommon : Mob
{
    private readonly int _uncommon;
    private int _walkStep;
    private int _lastNonZeroWalkStep;
    private const double _frameDuration = 1.0 / 8.0;
    private double _timeSinceLastFrameUpdate;
    
    protected Uncommon(int uncommon, int health, int speed, int xPosition, int yPosition) : base(health, speed, xPosition, yPosition)
    {
        _uncommon = uncommon;
        _walkStep = 0;
        _lastNonZeroWalkStep = 0;
        _timeSinceLastFrameUpdate = 0.0;
    }
    
    public override void Update(double elapsedTime)
    {
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
        Bitmap animationCycleBitmap = Art.Uncommons[_uncommon][_walkStep];
        screen.Draw(animationCycleBitmap, XPosition, YPosition);
    }
}