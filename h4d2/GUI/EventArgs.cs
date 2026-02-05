using H4D2.Spawners.SpecialSpawners;

namespace H4D2.GUI;

public class LevelSelectedEventArgs : EventArgs
{
    public readonly int Level;
    public LevelSelectedEventArgs(int level)
    {
        Level = level;
    }
}

public class LevelsSelectedEventArgs : EventArgs
{
    public readonly int Page;
    public readonly bool FromLevelComplete;
    public LevelsSelectedEventArgs(int page, bool fromLevelComplete)
    {
        Page = page;
        FromLevelComplete = fromLevelComplete;
    }
}

public class PauseToggleEventArgs : EventArgs
{
    public readonly ISpecialSpawnerView SpawnerView;
    public PauseToggleEventArgs(ISpecialSpawnerView spawnerView)
    {
        SpawnerView = spawnerView;
    }
}

public class MusicVolumeChangedEventArgs : EventArgs
{
    public readonly double MusicVolume;
    public MusicVolumeChangedEventArgs(double musicVolume)
    {
        MusicVolume = musicVolume;
    }
}

public class SFXVolumeChangedEventArgs : EventArgs
{
    public readonly double SFXVolume;
    public SFXVolumeChangedEventArgs(double sfxVolume)
    {
        SFXVolume = sfxVolume;
    }
}

public class FullscreenStateChangedEventArgs : EventArgs
{
    public readonly bool FullscreenEnabled;
    public FullscreenStateChangedEventArgs(bool fullscreenEnabled)
    {
        FullscreenEnabled = fullscreenEnabled;
    }
}