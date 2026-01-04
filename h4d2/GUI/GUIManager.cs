using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.GUI.Menus;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2.GUI;

public class GUIManager
{
    public event Func<int, SpecialSpawner>? LevelChangeRequested;
    public event EventHandler<MusicVolumeChangedEventArgs>? MusicVolumeChangeRequested;
    public event EventHandler<SFXVolumeChangedEventArgs>? SFXVolumeChangeRequested;
    public event EventHandler? PauseRequested;
    public event EventHandler? UnpauseRequested;
    public event EventHandler? ReloadMainMenuRequested;
    public event EventHandler? ExitRequested;

    private readonly SaveManager _saveManager;
    private readonly int _width;
    private readonly int _height;
    private Menu _menu;
    
    public GUIManager(SaveManager saveManager, int width, int height)
    {
        _saveManager = saveManager;
        _width = width;
        _height = height;
        _menu = new MainMenu(width, height);
        _menu.LevelsSelected += _OnLevelsSelected;
        _menu.SettingsSelected += _NavigateToSettings;
        _menu.ExitSelected += _OnExitSelected;
    }
    
    public void Update(Input input)
    {
        _menu.Update(input);
    }

    public void Render(Bitmap screen)
    {
        _menu.Render(screen);
    }

    public void ForceNavigateToLevelCompleteMenu(int levelID, double totalElapsedTime)
    {
        _menu = new LevelCompleteMenu(levelID, totalElapsedTime, _width, _height);
        _menu.LevelsSelected += _OnLevelsSelected;
    }
    
    private void _NavigateToLevels(int page)
    {
        _menu = new LevelsMenu(_saveManager, _width, _height, page);
        _menu.MainMenuSelected += _NavigateToMainMenu;
        _menu.LevelSelected += _OnLevelSelected;
    }
    
    private void _NavigateToMainMenu(object? sender, EventArgs e)
    {
        _menu = new MainMenu(_width, _height);
        _menu.LevelsSelected += _OnLevelsSelected;
        _menu.SettingsSelected += _NavigateToSettings;
        _menu.ExitSelected += _OnExitSelected;
    }
    
    private void _NavigateToSettings(object? sender, EventArgs e)
    {
        _menu = new SettingsMenu(
            _width,
            _height,
            _saveManager.GetMusicVolume(),
            _saveManager.GetSFXVolume()
        );
        _menu.MainMenuSelected += _NavigateToMainMenu;
        _menu.MusicVolumeChanged += _OnMusicVolumeChanged;
        _menu.SFXVolumeChanged += _OnSFXVolumeChanged;
    }

    private void _NavigateToHUD(ISpecialSpawnerView spawnerView)
    {
        _menu = new HUD(spawnerView, _width, _height);
        _menu.PauseSelected += _OnPauseSelected;
    }

    private void _NavigateToPauseMenu(ISpecialSpawnerView spawnerView)
    {
        _menu = new PauseMenu(spawnerView, _width, _height);
        _menu.UnpauseSelected += _OnUnpauseSelected;
        _menu.MainMenuSelected += _OnMainMenuSelected;
    }
    
    private void _OnLevelSelected(object? sender, LevelSelectedEventArgs e)
    {
        ISpecialSpawnerView? spawnerView = LevelChangeRequested?.Invoke(e.Level);
        if (spawnerView == null)
            return;
        _NavigateToHUD(spawnerView);
    }

    private void _OnLevelsSelected(object? sender, LevelsSelectedEventArgs e)
    {
        if(e.FromLevelComplete)
            ReloadMainMenuRequested?.Invoke(this, EventArgs.Empty);
        _NavigateToLevels(e.Page);
    }
    
    private void _OnPauseSelected(object? sender, PauseToggleEventArgs e)
    {
        _NavigateToPauseMenu(e.SpawnerView);
        PauseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void _OnUnpauseSelected(object? sender, PauseToggleEventArgs e)
    {
        _NavigateToHUD(e.SpawnerView);
        UnpauseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void _OnMainMenuSelected(object? sender, EventArgs e)
    {
        ReloadMainMenuRequested?.Invoke(this, EventArgs.Empty);
        _NavigateToMainMenu(sender, e);
    }
    
    private void _OnMusicVolumeChanged(object? sender, MusicVolumeChangedEventArgs e) =>
        MusicVolumeChangeRequested?.Invoke(this, e);
    
    private void _OnSFXVolumeChanged(object? sender, SFXVolumeChangedEventArgs e) =>
        SFXVolumeChangeRequested?.Invoke(this, e);
    
    private void _OnExitSelected(object? sender, EventArgs e) =>
        ExitRequested?.Invoke(this, EventArgs.Empty);
}