using H4D2.GUI.GUIParticles;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI.Menus;

public class LevelCompleteMenu : Menu
{
    private const int _paddingY = 10;
    
    private readonly int _levelID;

    private readonly CenteredHeader _centeredHeader;
    private readonly CenteredSubheader _centeredElapsedTime;
    private readonly Button _levelsButton;
    private ConfettiEmitter? _confettiEmitter;

    
    public LevelCompleteMenu(int levelID, double totalElapsedTime, int width, int height) 
        : base(width, height)
    {
        _levelID = levelID;
        
        int headerY = _height - (_height / 3);
        _centeredHeader = new CenteredHeader("Level Complete", headerY, _textColor);
        
        string time = TimeFormatter.Format(totalElapsedTime);
        int elapsedTimeY = headerY - (H4D2Art.GUI.TextHeight * 2) - _paddingY;
        _centeredElapsedTime = new CenteredSubheader($"Time taken: {time}", elapsedTimeY, _textColor);
        
        int levelsButtonY = (_height - (_height / 2)) - H4D2Art.LargeButtonHeight;
        _levelsButton = new Button(ButtonType.Levels, _centeredLargeButtonX, levelsButtonY);
        _levelsButton.Clicked += _OnLevelsButtonClicked;
        
        (int confettiX, int confettiY) = _GetRandomConfettiEmitterLocation();
        _confettiEmitter = new ConfettiEmitter(confettiX, confettiY);
    }

    public override void Update(Input input, double elapsedTime)
    {
        _confettiEmitter?.Update(elapsedTime);
        if (_confettiEmitter != null && _confettiEmitter.Removed)
        {
            (int confettiX, int confettiY) = _GetRandomConfettiEmitterLocation();
            _confettiEmitter = new ConfettiEmitter(confettiX, confettiY);
        }
        
        _levelsButton.Update(input);
    }

    public override void Render(H4D2BitmapCanvas screen)
    {
        _centeredHeader.Render(screen);
        _centeredElapsedTime.Render(screen);
        _levelsButton.Render(screen);
        _confettiEmitter?.Render(screen);
    }
    
    private (int, int) _GetRandomConfettiEmitterLocation()
    {
        int lowerXBound = _width / 4;
        int upperXBound = _width - (_width / 4);
        int lowerYBound = _height / 4;
        int upperYBound = _height - (_height / 4);
        double xScalar = RandomSingleton.Instance.NextDouble();
        double yScalar = RandomSingleton.Instance.NextDouble();
        double x = xScalar * (upperXBound - lowerXBound) + lowerXBound;
        double y = yScalar * (upperYBound - lowerYBound) + lowerYBound;
        return ((int)x, (int)y);
    }

    private void _OnLevelsButtonClicked(object? sender, EventArgs e)
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonDefault);
        _RaiseLevelsSelectedFromLevelComplete(_levelID);
    }
}