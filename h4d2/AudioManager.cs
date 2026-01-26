using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using SFML.Audio;
using SFML.System;

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
    private Vector3f _sfxPosition;
    private Camera? _camera;
    private bool _isInGame;
    
    private AudioManager()
    {
        MusicVolume = MaxVolume;
        SFXVolume = MaxVolume;
        
        _musics = new Dictionary<Track, Music>();
        foreach (KeyValuePair<Track, string> kvp in AudioResources.TrackPaths)
        {
            _musics[kvp.Key] = new Music(kvp.Value);
            _musics[kvp.Key].Loop = true;
        }
        
        _sounds = new Dictionary<SFX, Sound>();
        foreach (KeyValuePair<SFX, string> kvp in AudioResources.SFXPaths)
        {
            _sounds[kvp.Key] = new Sound(new SoundBuffer(kvp.Value));
            _sounds[kvp.Key].Loop = false;
        }
        
        _currentMusic = null;
        _sfxPosition = new Vector3f(0, 0, -1);
        _camera = null;
        _isInGame = false;
    }

    public void SetCamera(Camera camera)
    {
        // i have a setter here intead of making _camera public because 
        // AudioManager is a global singleton and the camera object doesn't need to be
        // accessible from everywhere
        _camera = camera;
    }

    public void SetInGameState(bool isInGame)
    {
        _isInGame = isInGame;
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
        if (!_musics.TryGetValue(track, out var music))
            return;
        if (music == _currentMusic)
            return;
        _currentMusic?.Stop();
        music.Volume = _SFMLVolume(MusicVolume);
        music.Play();
        _currentMusic = music;
    }

    public void PauseMusic() => _currentMusic?.Pause();
    public void UnpauseMusic() => _currentMusic?.Play();
    
    public void PlaySFX(
        SFX sfx,
        int xScreenPos = (int)H4D2.ScreenWidth / 2,
        int yScreenPos = (int)H4D2.ScreenHeight / 2
    )
    {
        if (!_sounds.TryGetValue(sfx, out var sound))
            return;
        
        bool isUISound = AudioResources.SoundsUnaffectedByCamera.Contains(sfx);
        if (!isUISound && !_isInGame)
            return;
        
        if (_camera != null && !isUISound)
        {
            xScreenPos += _camera.XOffset;
            yScreenPos += _camera.YOffset;
        }
     
        if (!_IsSoundOnScreen(xScreenPos, yScreenPos))
            return;
        
        (float x, float y) = _ConvertScreenPosToPan(xScreenPos, yScreenPos);
        _sfxPosition.X = x;
        _sfxPosition.Y = y;
        sound.Position = _sfxPosition;
        sound.Volume = _SFMLVolume(SFXVolume * (1 - Math.Abs(x)));
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
            kvp.Value.SoundBuffer.Dispose();
            kvp.Value.Dispose();
        }
    }
    
    private static float _SFMLVolume(double volume)
    {
        return (float)(volume * 100);
    }

    private static bool _IsSoundOnScreen(int xScreenPos, int yScreenPos)
    {
        const double padding = 48;
        if (
            xScreenPos + padding < 0 ||
            yScreenPos + padding < 0 ||
            xScreenPos - padding > H4D2.ScreenWidth || 
            yScreenPos - padding > H4D2.ScreenHeight
        )
            return false;
        return true;
    }

    private static (float, float) _ConvertScreenPosToPan(int xScreenPos, int yScreenPos)
    {
        const float maxValue = 0.75f;
        int xPos = xScreenPos - ((int)H4D2.ScreenWidth / 2);
        int yPos = yScreenPos - ((int)H4D2.ScreenHeight / 2);
        
        return (
            ((float)xPos / (int)H4D2.ScreenWidth) * maxValue,
            ((float)yPos / (int)H4D2.ScreenHeight) * maxValue
        );
    }
}