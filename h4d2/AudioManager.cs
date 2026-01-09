using H4D2.Infrastructure.H4D2;
using SFML.Audio;

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
    
    private readonly Dictionary<Track, Music> _musics;
    private readonly Dictionary<SFX, Sound> _sounds;
    private Music? _currentMusic;
    
    private AudioManager()
    {
        MusicVolume = MaxVolume;
        SFXVolume = MaxVolume;
        
        _musics = new Dictionary<Track, Music>();
        foreach (KeyValuePair<Track, string> kvp in AudioResources.LoadTrackPaths())
        {
            _musics[kvp.Key] = new Music(kvp.Value);
            _musics[kvp.Key].Loop = true;
        }
        
        _sounds = new Dictionary<SFX, Sound>();
        foreach (KeyValuePair<SFX, string> kvp in AudioResources.LoadSFXPaths())
        {
            _sounds[kvp.Key] = new Sound(new SoundBuffer(kvp.Value));
            _sounds[kvp.Key].Loop = false;
        }
        
        _currentMusic = null;
    }
    
    public void UpdateMusicVolume(double volume)
    {
        if (volume < MinVolume || volume > MaxVolume)
            return;
        MusicVolume = volume;
        if (_currentMusic == null)
            return;
        _currentMusic.Volume = _SFMLVolume(MusicVolume);
    }

    public void UpdateSFXVolume(double volume)
    {
        if (volume < MinVolume || volume > MaxVolume)
            return;
        SFXVolume = volume;
    }

    public void PlayMusic(Track track)
    {
        if (MusicVolume == 0.0)
            return;
        if (!_musics.TryGetValue(track, out var music))
            return;
        if (music == _currentMusic)
            return;
        _currentMusic?.Stop();
        music.Volume = _SFMLVolume(MusicVolume);
        music.Play();
        _currentMusic = music;
    }

    public void PlaySFX(SFX sfx)
    {
        if (SFXVolume == 0.0)
            return;
        if (!_sounds.TryGetValue(sfx, out var sound))
            return;
        sound.Volume = _SFMLVolume(SFXVolume);
        sound.Play();
    }

    public void Close()
    {
        foreach (KeyValuePair<Track, Music> kvp in _musics)
        {
            kvp.Value.Dispose();
        }
        foreach (KeyValuePair<SFX, Sound> kvp in _sounds)
        {
            kvp.Value.Dispose();
        }
    }
    
    private static float _SFMLVolume(double volume)
    {
        return (float)(volume * 100);
    }
}