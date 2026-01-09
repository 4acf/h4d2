namespace H4D2.Infrastructure.H4D2;

public enum Track
{
    TheParish,
    PrayForDeath,
    DeadLightDistrict,
    SkinOnOurTeeth,
    Gallery,
    TheMonstersWithin,
    OneBadTank
}

public enum SFX
{
    Navigation1,
    Navigation2,
    Navigation3,
    Play,
    GUIButton,
    HUDButton,
    Explosion1,
    Explosion2,
    Death1,
    Death2,
    BoomerDeath,
    SmallGun,
    BigGun,
    HealthPickup,
    ThrowablePickup,
    SpecialSpawned,
}

public static class AudioResources
{
    public static Dictionary<Track, string> LoadTrackPaths()
        => new()
        {
            {Track.Gallery, $"{Resources.MusicPrefix}gallery.ogg"},
            {Track.OneBadTank, $"{Resources.MusicPrefix}onebadtank.ogg"}
        };

    public static Dictionary<SFX, string> LoadSFXPaths()
        => new()
        {

        };
}