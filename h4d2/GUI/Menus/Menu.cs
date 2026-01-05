using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2.GUI.Menus;

public abstract class Menu
{
    protected const int _textColor = 0xffffff;

    public event EventHandler<LevelSelectedEventArgs>? LevelSelected;
    public event EventHandler<LevelsSelectedEventArgs>? LevelsSelected;
    public event EventHandler? SettingsSelected;
    public event EventHandler? ExitSelected;
    public event EventHandler? MainMenuSelected;
    public event EventHandler<PauseToggleEventArgs>? PauseSelected;
    public event EventHandler<PauseToggleEventArgs>? UnpauseSelected;
    public event EventHandler<MusicVolumeChangedEventArgs>? MusicVolumeChanged;
    public event EventHandler<SFXVolumeChangedEventArgs>? SFXVolumeChanged;
    
    protected readonly int _width;
    protected readonly int _height;
    protected readonly int _centeredLargeButtonX;
    protected readonly int _centeredSmallButtonY;
    
    protected Menu(int width, int height)
    {
        _width = width;
        _height = height;
        _centeredLargeButtonX = (width / 2) - (H4D2Art.LargeButtonWidth / 2);
        _centeredSmallButtonY = (height / 2) + (H4D2Art.SmallButtonHeight / 2);
    }

    public abstract void Update(Input input, double elapsedTime);
    public abstract void Render(Bitmap screen);

    protected void _RaiseLevelSelected(int page) =>
        LevelSelected?.Invoke(this, new LevelSelectedEventArgs(page));
    protected void _RaiseLevelsSelected(int page = 0) =>
        LevelsSelected?.Invoke(this, new LevelsSelectedEventArgs(page, false));
    protected void _RaiseLevelsSelectedFromLevelComplete(int page = 0) =>
        LevelsSelected?.Invoke(this, new LevelsSelectedEventArgs(page, true));
    protected void _RaiseSettingsSelected() =>
        SettingsSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaiseExitSelected() =>
        ExitSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaiseMainMenuSelected() =>
        MainMenuSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaisePauseSelected(ISpecialSpawnerView spawnerView) =>
        PauseSelected?.Invoke(this, new PauseToggleEventArgs(spawnerView));
    protected void _RaiseUnpauseSelected(ISpecialSpawnerView spawnerView) =>
        UnpauseSelected?.Invoke(this, new PauseToggleEventArgs(spawnerView));
    protected void _RaiseMusicVolumeChanged(double volume) =>
        MusicVolumeChanged?.Invoke(this, new MusicVolumeChangedEventArgs(volume));
    protected void _RaiseSFXVolumeChanged(double volume) =>
        SFXVolumeChanged?.Invoke(this, new SFXVolumeChangedEventArgs(volume));
}