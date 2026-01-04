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
    
    public LevelCompleteMenu(int levelID, double totalElapsedTime, int width, int height) 
        : base(width, height)
    {
        _levelID = levelID;
        
        int headerY = _height - (_height / 3);
        _centeredHeader = new CenteredHeader("Level Complete", headerY, _textColor);
        
        string time = TimeFormatter.Format(totalElapsedTime);
        int elapsedTimeY = headerY - (H4D2Art.TextHeight * 2) - _paddingY;
        _centeredElapsedTime = new CenteredSubheader($"Time taken: {time}", elapsedTimeY, _textColor);
        
        int levelsButtonY = (_height - (_height / 2)) - H4D2Art.LargeButtonHeight;
        _levelsButton = new Button(ButtonType.Levels, _centeredLargeButtonX, levelsButtonY);
        _levelsButton.Clicked += _OnLevelsButtonClicked;
    }

    public override void Update(Input input)
    {
        _levelsButton.Update(input);
    }

    public override void Render(Bitmap screen)
    {
        _centeredHeader.Render(screen);
        _centeredElapsedTime.Render(screen);
        _levelsButton.Render(screen);
    }
    
    private void _OnLevelsButtonClicked(object? sender, EventArgs e) =>
        _RaiseLevelsSelectedFromLevelComplete(_levelID);
}