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
    ButtonDefault,
    ButtonNavigation1,
    ButtonNavigation2,
    ButtonNavigation3,
    ButtonPlay,
    ExplosionSmall,
    ExplosionLarge,
    Death1,
    Death2,
    Death3,
    WeaponSmall,
    WeaponLarge,
    PickupConsumable,
    PickupThrowable
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
            {SFX.ButtonDefault , $"{Resources.SFXPrefix}button_default.wav"},
            {SFX.ButtonNavigation1 , $"{Resources.SFXPrefix}button_nav_1.wav"},
            {SFX.ButtonNavigation2 , $"{Resources.SFXPrefix}button_nav_2.wav"},
            {SFX.ButtonNavigation3 , $"{Resources.SFXPrefix}button_nav_3.wav"},
            {SFX.ButtonPlay , $"{Resources.SFXPrefix}button_play.wav"},
            {SFX.ExplosionSmall, $"{Resources.SFXPrefix}explosion_small.wav"},
            {SFX.ExplosionLarge, $"{Resources.SFXPrefix}explosion_large.wav"},
            {SFX.Death1, $"{Resources.SFXPrefix}death_1.wav"},
            {SFX.Death2, $"{Resources.SFXPrefix}death_2.wav"},
            {SFX.Death3, $"{Resources.SFXPrefix}death_3.wav"},
            {SFX.WeaponSmall, $"{Resources.SFXPrefix}weapon_small.wav"},
            {SFX.WeaponLarge, $"{Resources.SFXPrefix}weapon_large.wav"},
            {SFX.PickupConsumable, $"{Resources.SFXPrefix}pickup_consumable.wav"},
            {SFX.PickupThrowable, $"{Resources.SFXPrefix}pickup_throwable.wav"}
        };
}