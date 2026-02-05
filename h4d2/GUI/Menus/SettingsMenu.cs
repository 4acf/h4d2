using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI.Menus;

public class SettingsMenu : Menu
{
    private const int _paddingBetweenX = 8;
    private const int _paddingBetweenY = 20;
    private const int _paddingBelowHeader = 30;
    private readonly CenteredHeader _header;
    private readonly Subheader _musicVolumeSubheader;
    private readonly Subheader _sfxVolumeSubheader;
    private readonly Subheader _fullscreenSubheader;
    private readonly VolumeSelector _musicVolumeSelector;
    private readonly VolumeSelector _sfxVolumeSelector;
    private readonly Checkbox _fullscreenCheckbox;
    private readonly Button _mainMenuButton;
    
    public SettingsMenu(int width, int height, double musicVolume, double sfxVolume, bool fullscreenEnabled) 
        : base(width, height)
    {
        int headerY = _height - (_height / 3);
        _header = new CenteredHeader("Settings", headerY, _textColor);
        
        int musicVolumeY = headerY - _paddingBelowHeader;
        int sfxVolumeY = musicVolumeY - _paddingBetweenY;
        int fullscreenY = sfxVolumeY - _paddingBetweenY;
        
        const string musicVolumeSubheaderText = "Music Volume";
        int musicVolumeSubheaderWidth = Pixuf.GetTextWidth(musicVolumeSubheaderText);
        int headerPlusSelectorWidth = musicVolumeSubheaderWidth + _paddingBetweenX + VolumeSelector.Width;

        int subheaderX = (width / 2) - (headerPlusSelectorWidth / 2);
        
        _musicVolumeSubheader = new Subheader(musicVolumeSubheaderText, subheaderX, musicVolumeY, _textColor);
        _sfxVolumeSubheader = new Subheader("SFX Volume", subheaderX, sfxVolumeY, _textColor);
        _fullscreenSubheader = new Subheader("Fullscreen", subheaderX, fullscreenY, _textColor);
        
        int volumeSelectorX = subheaderX + _paddingBetweenX + musicVolumeSubheaderWidth;
        _musicVolumeSelector = new VolumeSelector(volumeSelectorX, musicVolumeY + 1, musicVolume);
        _musicVolumeSelector.ValueUpdated += _OnMusicVolumeSelectorUpdated;
        _sfxVolumeSelector = new VolumeSelector(volumeSelectorX, sfxVolumeY + 1, sfxVolume);
        _sfxVolumeSelector.ValueUpdated += _OnSFXVolumeSelectorUpdated;

        int fullscreenCheckboxX = 
            volumeSelectorX + VolumeSelector.Width - Checkbox.Size - VolumeSelector.PaddingBetween;
        int fullscreenCheckboxY = fullscreenY + 1;
        _fullscreenCheckbox = new Checkbox(fullscreenCheckboxX, fullscreenCheckboxY, fullscreenEnabled);
        _fullscreenCheckbox.ValueUpdated += _OnFullscreenStateUpdated;
        
        int mainMenuButtonY = (height / 3) - _paddingBetweenY;
        _mainMenuButton = new Button(ButtonType.MainMenu, _centeredLargeButtonX, mainMenuButtonY);
        _mainMenuButton.Clicked += _OnMainMenuButtonClicked;
    }

    public override void Update(Input input, double elapsedTime)
    {
        if (input.IsEscPressed)
        {
            AudioManager.Instance.PlaySFX(SFX.ButtonDefault);
            _RaiseMainMenuSelected();
            return;
        }
        
        _musicVolumeSelector.Update(input);
        _sfxVolumeSelector.Update(input);
        _fullscreenCheckbox.Update(input);
        _mainMenuButton.Update(input);
    }

    public override void Render(H4D2BitmapCanvas screen)
    {
        _header.Render(screen);
        _musicVolumeSubheader.Render(screen);
        _sfxVolumeSubheader.Render(screen);
        _fullscreenSubheader.Render(screen);
        _musicVolumeSelector.Render(screen);
        _sfxVolumeSelector.Render(screen);
        _fullscreenCheckbox.Render(screen);
        _mainMenuButton.Render(screen);
    }

    private void _OnMusicVolumeSelectorUpdated(object? sender, EventArgs e) =>
        _RaiseMusicVolumeChanged(_musicVolumeSelector.GetVolume());

    private void _OnSFXVolumeSelectorUpdated(object? sender, EventArgs e) =>
        _RaiseSFXVolumeChanged(_sfxVolumeSelector.GetVolume());

    private void _OnFullscreenStateUpdated(object? sender, EventArgs e) =>
        _RaiseFullscreenStateChanged(_fullscreenCheckbox.IsEnabled);
    
    private void _OnMainMenuButtonClicked(object? sender, EventArgs e)
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonDefault);
        _RaiseMainMenuSelected();
    }
}