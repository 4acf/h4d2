using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.UI.Menus;

public abstract class Menu
{
    public event EventHandler? LevelsSelected;
    public event EventHandler? SettingsSelected;
    public event EventHandler? ExitSelected;
    
    protected readonly int _width;
    protected readonly int _height;
    protected readonly int _centeredLargeButtonX;
    
    protected Menu(int width, int height)
    {
        _width = width;
        _height = height;
        _centeredLargeButtonX = (width / 2) - (H4D2Art.LargeButtonWidth / 2);
    }

    public abstract void Update(Input input);
    public abstract void Render(Bitmap screen);

    
    protected void _RaiseLevelsSelected() =>
        LevelsSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaiseSettingsSelected() =>
        SettingsSelected?.Invoke(this, EventArgs.Empty);
    protected void _RaiseExitSelected() =>
        ExitSelected?.Invoke(this, EventArgs.Empty);
}