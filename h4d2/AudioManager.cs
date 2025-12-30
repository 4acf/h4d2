namespace H4D2;

public class AudioManager
{
    public const double MinVolume = 0.0;
    public const double MaxVolume = 1.0;
    
    public double MusicVolume { get; private set; }
    public double SFXVolume { get; private set; }
    
    public AudioManager(double musicVolume, double sfxVolume)
    {
        MusicVolume = musicVolume;
        SFXVolume = sfxVolume;
    }

    public void UpdateMusicVolume(double volume)
    {
        if (volume < MinVolume || volume > MaxVolume)
            return;
        MusicVolume = volume;
    }

    public void UpdateSFXVolume(double volume)
    {
        if (volume < MinVolume || volume > MaxVolume)
            return;
        SFXVolume = volume;
    }
}