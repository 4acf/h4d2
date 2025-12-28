using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.UI.Menus;

public class MainMenu : Menu
{
    private const int _padding = 20;
    private const int _buttonPaddingBetween = 2;
    
    private readonly Button _playButton;
    private readonly Button _settingsButton;
    private readonly Button _exitButton;
    
    public MainMenu(int width, int height) : base(width, height)
    {
        int playButtonY = height - H4D2Art.Title.Height - _padding;
        int settingsButtonY = playButtonY - H4D2Art.LargeButtonHeight - _buttonPaddingBetween;
        int exitButtonY = settingsButtonY - H4D2Art.LargeButtonHeight - _buttonPaddingBetween;
        
        _playButton = new Button(ButtonType.Play, _centeredLargeButtonX, playButtonY);
        _settingsButton = new Button(ButtonType.Settings, _centeredLargeButtonX, settingsButtonY);
        _exitButton = new Button(ButtonType.Exit, _centeredLargeButtonX, exitButtonY);
    }

    public override void Update(Input input)
    {
        _playButton.Update(input);
        _settingsButton.Update(input);
        _exitButton.Update(input);
    }

    public override void Render(Bitmap screen)
    {
        Bitmap titleBitmap = H4D2Art.Title;
        int titleCenteredX = (_width / 2) - (titleBitmap.Width / 2);
        screen.DrawAbsolute(titleBitmap, titleCenteredX, _height - _padding);
        
        _playButton.Render(screen);
        _settingsButton.Render(screen);
        _exitButton.Render(screen);
    }
}