using H4D2.Infrastructure;
using H4D2.UI.Menus;

namespace H4D2.UI;

public class UIManager
{
    public event EventHandler? ExitRequested;
    
    private readonly int _width;
    private readonly int _height;
    private Menu _menu;
    
    public UIManager(int width, int height)
    {
        _width = width;
        _height = height;
        _menu = new MainMenu(width, height);
        _menu.SettingsSelected += _OnSettingsSelected;
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

    private void _NavigateToMainMenu()
    {
        _menu = new MainMenu(_width, _height);
        _menu.SettingsSelected += _OnSettingsSelected;
        _menu.ExitSelected += _OnExitSelected;
    }
    
    private void _NavigateToSettings()
    {
        _menu = new SettingsMenu(_width, _height);
        _menu.MainMenuSelected += OnMainMenuSelected;
    }
    
    private void _OnSettingsSelected(object? sender, EventArgs e) =>
        _NavigateToSettings();
    private void _OnExitSelected(object? sender, EventArgs e) =>
        ExitRequested?.Invoke(this, EventArgs.Empty);
    private void OnMainMenuSelected(object? sender, EventArgs e) =>
        _NavigateToMainMenu();
}