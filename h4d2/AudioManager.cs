namespace H4D2;

public sealed class AudioManager
{
    public const double MinVolume = 0.0;
    public const double MaxVolume = 1.0;
    
    public double MusicVolume { get; private set; }
    public double SFXVolume { get; private set; }

    private static readonly Lazy<AudioManager> _instance =
        new(() => new AudioManager());
    
    public static AudioManager Instance => _instance.Value;
    
    private AudioManager()
    {
        MusicVolume = MaxVolume;
        SFXVolume = MaxVolume;
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