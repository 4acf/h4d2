using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.UI.Menus;

public class LevelsMenu : Menu
{
    private const int _xEdgePadding = 5;
    private const int _paddingBetweenButtonsY = 2;
    private const int _mainMenuButtonYOffset = 20;
    private readonly CenteredHeader _levelNameHeader;
    private readonly Button _backwardNavigationButton;
    private readonly Button _forwardNavigationButton;
    private readonly Button _playButton;
    private readonly Button _mainMenuButton;
    private int _page;
    
    public LevelsMenu(int width, int height, int page = 0) : base(width, height)
    {
        if (page < 0 || page >= LevelCollection.NumLevels)
            page = 0;
        _page = page;

        int headerY = _height - (_height / 3);
        _levelNameHeader = new CenteredHeader(LevelCollection.Levels[_page].Name, headerY, _textColor);
        
        _backwardNavigationButton = new Button(ButtonType.Backward, _xEdgePadding, _centeredSmallButtonY);
        _backwardNavigationButton.Clicked += (_, _) =>
        {
            _page--;
            _RefreshLevelDetails();
        };
        
        int forwardButtonX = width - _xEdgePadding - H4D2Art.SmallButtonWidth;
        _forwardNavigationButton = new Button(ButtonType.Forward, forwardButtonX, _centeredSmallButtonY);
        _forwardNavigationButton.Clicked += (_, _) =>
        {
            _page++;
            _RefreshLevelDetails();
        };
        
        int mainMenuButtonY = (height / 3) - _mainMenuButtonYOffset;
        _mainMenuButton = new Button(ButtonType.MainMenu, _centeredLargeButtonX, mainMenuButtonY);
        _mainMenuButton.Clicked += _OnMainMenuButtonClicked;
        
        int playButtonY = mainMenuButtonY + H4D2Art.LargeButtonHeight + _paddingBetweenButtonsY;
        _playButton = new Button(ButtonType.Play, _centeredLargeButtonX, playButtonY);
    }

    public override void Update(Input input)
    {
        if(_page > 0)
            _backwardNavigationButton.Update(input);
        
        if(_page < LevelCollection.NumLevels - 1)
            _forwardNavigationButton.Update(input);
        
        _playButton.Update(input);
        _mainMenuButton.Update(input);
    }

    public override void Render(Bitmap screen)
    {
        if(_page > 0)
            _backwardNavigationButton.Render(screen);
        
        if(_page < LevelCollection.NumLevels - 1)
            _forwardNavigationButton.Render(screen);
     
        _levelNameHeader.Render(screen);
        _playButton.Render(screen);
        _mainMenuButton.Render(screen);
    }

    private void _RefreshLevelDetails()
    {
        _levelNameHeader.UpdateText(LevelCollection.Levels[_page].Name);
    }
    
    private void _OnMainMenuButtonClicked(object? sender, EventArgs e) =>
        _RaiseMainMenuSelected();
}