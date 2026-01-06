using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI.Menus;

public class MainMenu : Menu
{
    private const int _padding = 20;
    private const int _buttonPaddingBetween = 2;

    private readonly Button _playButton;
    private readonly Button _settingsButton;
    private readonly Button _exitButton;

    public MainMenu(int width, int height) : base(width, height)
    {
        int playButtonY = height - H4D2Art.GUI.Title.Height - _padding;
        int settingsButtonY = playButtonY - H4D2Art.LargeButtonHeight - _buttonPaddingBetween;
        int exitButtonY = settingsButtonY - H4D2Art.LargeButtonHeight - _buttonPaddingBetween;

        _playButton = new Button(ButtonType.Play, _centeredLargeButtonX, playButtonY);
        _playButton.Clicked += _OnPlayButtonClicked;
        _settingsButton = new Button(ButtonType.Settings, _centeredLargeButtonX, settingsButtonY);
        _settingsButton.Clicked += _OnSettingsButtonClicked;
        _exitButton = new Button(ButtonType.Exit, _centeredLargeButtonX, exitButtonY);
        _exitButton.Clicked += _OnExitButtonClicked;
    }

    public override void Update(Input input, double elapsedTime)
    {
        _playButton.Update(input);
        _settingsButton.Update(input);
        _exitButton.Update(input);
    }

    public override void Render(Bitmap screen)
    {
        Bitmap titleBitmap = H4D2Art.GUI.Title;
        int titleCenteredX = (_width / 2) - (titleBitmap.Width / 2);
        screen.DrawAbsolute(titleBitmap, titleCenteredX, _height - _padding);

        _playButton.Render(screen);
        _settingsButton.Render(screen);
        _exitButton.Render(screen);
    }

    private void _OnPlayButtonClicked(object? sender, EventArgs e) =>
        _RaiseLevelsSelected();
    private void _OnSettingsButtonClicked(object? sender, EventArgs e) =>
        _RaiseSettingsSelected();
    private void _OnExitButtonClicked(object? sender, EventArgs e) =>
        _RaiseExitSelected();
}