using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.UI.Menus;

public abstract class Menu
{
    protected const int _textColor = 0xffffff;
    
    public event EventHandler? LevelsSelected;
    public event EventHandler? SettingsSelected;
    public event EventHandler? ExitSelected;
    public event EventHandler? MainMenuSelected;
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
        _centeredSmallButtonY = (height / 2) + (H4D2Art.SmallButtonSize / 2);
    }

    public abstract void Update(Input input);
    public abstract void Render(Bitmap screen);

    
    protected void _RaiseLevelsSelected() =>
        LevelsSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaiseSettingsSelected() =>
        SettingsSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaiseExitSelected() =>
        ExitSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaiseMainMenuSelected() =>
        MainMenuSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaiseMusicVolumeChanged(double volume) =>
        MusicVolumeChanged?.Invoke(this, new MusicVolumeChangedEventArgs(volume));
    protected void _RaiseSFXVolumeChanged(double volume) =>
        SFXVolumeChanged?.Invoke(this, new SFXVolumeChangedEventArgs(volume));
}