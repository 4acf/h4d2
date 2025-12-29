using H4D2.Infrastructure;

namespace H4D2.UI.Menus;

public class LevelsMenu : Menu
{
    private const int _paddingBetweenY = 20;

    
    private readonly Button _mainMenuButton;

    
    public LevelsMenu(int width, int height) : base(width, height)
    {
        int mainMenuButtonY = (height / 3) - _paddingBetweenY;
        _mainMenuButton = new Button(ButtonType.MainMenu, _centeredLargeButtonX, mainMenuButtonY);
        _mainMenuButton.Clicked += _OnMainMenuButtonClicked;
    }

    public override void Update(Input input)
    {
        _mainMenuButton.Update(input);
    }

    public override void Render(Bitmap screen)
    {
        _mainMenuButton.Render(screen);
    }
    
    private void _OnMainMenuButtonClicked(object? sender, EventArgs e) =>
        _RaiseMainMenuSelected();
}