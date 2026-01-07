using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2.GUI.Menus;

public class LevelsMenu : Menu
{
    private const int _xEdgePadding = 5;
    private const int _paddingBetweenButtonsY = 2;
    private const int _paddingBetweenHeadersY = 20;
    private const int _mainMenuButtonYOffset = 20;
    private readonly SaveManager _saveManager;
    private readonly CenteredHeader _levelNameHeader;
    private readonly CenteredSubheader _recordSubheader;
    private readonly Button _backwardNavigationButton;
    private readonly Button _forwardNavigationButton;
    private readonly Button _playButton;
    private readonly Button _mainMenuButton;
    private readonly PageViewer _pageViewer;
    private int _page;
    
    public LevelsMenu(SaveManager saveManager, int width, int height, int page = 0) : base(width, height)
    {
        _saveManager = saveManager;
        
        if (page < 0 || page >= LevelCollection.NumLevels)
            page = 0;
        _page = page;

        int headerY = _height - (_height / 3);
        _levelNameHeader = new CenteredHeader(LevelCollection.Levels[_page].Name, headerY, _textColor);

        int recordY = headerY - (H4D2Art.GUI.TextHeight * 2) - _paddingBetweenHeadersY;
        string record = _GetRecordText(_page);
        _recordSubheader = new CenteredSubheader(record, recordY, _textColor);
        
        _backwardNavigationButton = new Button(ButtonType.Backward, _xEdgePadding, _centeredSmallButtonY);
        _backwardNavigationButton.Clicked += (_, _) =>
        {
            _page = (LevelCollection.NumLevels + (_page - 1)) % LevelCollection.NumLevels;
            _RefreshPageDetails();
        };
        
        int forwardButtonX = width - _xEdgePadding - H4D2Art.SmallButtonWidth;
        _forwardNavigationButton = new Button(ButtonType.Forward, forwardButtonX, _centeredSmallButtonY);
        _forwardNavigationButton.Clicked += (_, _) =>
        {
            _page = (_page + 1) % LevelCollection.NumLevels;
            _RefreshPageDetails();
        };
        
        int mainMenuButtonY = (height / 3) - _mainMenuButtonYOffset;
        _mainMenuButton = new Button(ButtonType.MainMenu, _centeredLargeButtonX, mainMenuButtonY);
        _mainMenuButton.Clicked += _OnMainMenuButtonClicked;
        
        int playButtonY = mainMenuButtonY + H4D2Art.LargeButtonHeight + _paddingBetweenButtonsY;
        _playButton = new Button(ButtonType.Play, _centeredLargeButtonX, playButtonY);
        _playButton.Clicked += _OnPlayButtonClicked;

        _pageViewer = new PageViewer(
            LevelCollection.NumLevels,
            _height - PageViewer.Scale * 2,
            width,
            page
        );
    }

    public override void Update(Input input, double elapsedTime)
    {
        _backwardNavigationButton.Update(input);
        _forwardNavigationButton.Update(input);
        _playButton.Update(input);
        _mainMenuButton.Update(input);
    }

    public override void Render(H4D2BitmapCanvas screen)
    {
        _backwardNavigationButton.Render(screen);
        _forwardNavigationButton.Render(screen);
        _levelNameHeader.Render(screen);
        _recordSubheader.Render(screen);
        _playButton.Render(screen);
        _mainMenuButton.Render(screen);
        _pageViewer.Render(screen);
    }

    private void _RefreshPageDetails()
    {
        _levelNameHeader.UpdateText(LevelCollection.Levels[_page].Name);
        _recordSubheader.UpdateText(_GetRecordText(_page));
        _pageViewer.Update(_page);
    }

    private string _GetRecordText(int page)
    {
        double? record = _saveManager.GetLevelRecord(page);
        if (record == null)
            return "LEVEL NOT COMPLETED";
        string formatted = TimeFormatter.Format(record.Value);
        return $"Best Time: {formatted}";
    }
    
    private void _OnPlayButtonClicked(object? sender, EventArgs e)
        => _RaiseLevelSelected(_page);
    
    private void _OnMainMenuButtonClicked(object? sender, EventArgs e) =>
        _RaiseMainMenuSelected();
}