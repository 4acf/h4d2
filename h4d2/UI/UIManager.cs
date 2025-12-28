using H4D2.Infrastructure;
using H4D2.UI.Menus;

namespace H4D2.UI;

public class UIManager
{
    private readonly int _width;
    private readonly int _height;
    private Menu _menu;
    
    public UIManager(int width, int height)
    {
        _width = width;
        _height = height;
        _menu = new MainMenu(width, height);
    }

    public void Update(Input input)
    {
        _menu.Update(input);
    }

    public void Render(Bitmap screen)
    {
        _menu.Render(screen);
    }
}