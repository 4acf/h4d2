using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2.GUI.Menus;

public class PauseMenu : Menu
{
    private const int _paddingBetweenY = 2;
    
    private readonly ISpecialSpawnerView _spawnerView;
    private readonly CenteredHeader _centeredHeader;
    private readonly Button _resumeButton;
    private readonly Button _mainMenuButton;
    
    public PauseMenu(ISpecialSpawnerView spawnerView, int width, int height) : base(width, height)
    {
        AudioManager.Instance.PauseMusic();
        
        _spawnerView = spawnerView;
        
        int headerY = _height - (_height / 3);
        _centeredHeader = new CenteredHeader("Paused", headerY, _textColor);
        
        int resumeButtonY = _height - (_height / 2);
        _resumeButton = new Button(ButtonType.Resume, _centeredLargeButtonX, resumeButtonY);
        _resumeButton.Clicked += _OnResumeButtonClicked;
        
        int mainMenuButtonY = resumeButtonY - H4D2Art.LargeButtonHeight - _paddingBetweenY;
        _mainMenuButton = new Button(ButtonType.MainMenu, _centeredLargeButtonX, mainMenuButtonY);
        _mainMenuButton.Clicked += _OnMainMenuButtonClicked;
    }

    public override void Update(Input input, double elapsedTime)
    {
        if (input.IsEscPressed || input.IsEnterPressed)
        {
            _RaiseUnpauseSelected(_spawnerView);
            return;
        }
        
        _resumeButton.Update(input);
        _mainMenuButton.Update(input);
    }

    public override void Render(H4D2BitmapCanvas screen)
    {
        _centeredHeader.Render(screen);
        _resumeButton.Render(screen);
        _mainMenuButton.Render(screen);
    }

    private void _OnResumeButtonClicked(object? sender, EventArgs e)
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonDefault);
        _RaiseUnpauseSelected(_spawnerView);
    }

    private void _OnMainMenuButtonClicked(object? sender, EventArgs e)
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonDefault);
        AudioManager.Instance.UnpauseMusic();
        _RaiseMainMenuSelected();
    }
    
}