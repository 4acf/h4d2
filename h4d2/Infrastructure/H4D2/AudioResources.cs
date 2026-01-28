using System.Collections.Immutable;

namespace H4D2.Infrastructure.H4D2;

public enum Track
{
    DeadLightDistrict,
    Gallery,
    OneBadTank,
    SkinOnOurTeeth,
    TheMonstersWithin,
    TheParish
}

public enum SFX
{
    BileBombLand,
    ButtonDefault,
    ButtonPlay,
    Death1,
    Death2,
    Death3,
    Explosion1,
    Explosion2,
    Explosion3,
    ExplosionBoomer,
    Hit1,
    Hit2,
    Jump,
    MolotovLand,
    PickupConsumable,
    PickupThrowable,
    Puke,
    Slam1,
    Slam2,
    Slam3,
    SpecialSelect,
    SpecialSpawn,
    Spit,
    WeaponLarge1,
    WeaponLarge2,
    WeaponLarge3,
    WeaponSmall,
    WitchAlert
}

public static class AudioResources
{
    public static readonly ImmutableDictionary<Track, string> TrackPaths
        = new Dictionary<Track, string>
        {
            {Track.Gallery, Resources.MusicPrefix("gallery.ogg")},
            {Track.OneBadTank, Resources.MusicPrefix("one_bad_tank.ogg")},
            {Track.TheMonstersWithin, Resources.MusicPrefix("the_monsters_within.ogg")},
            {Track.TheParish, Resources.MusicPrefix("the_parish.ogg")},
            {Track.DeadLightDistrict, Resources.MusicPrefix("dead_light_district.ogg")},
            {Track.SkinOnOurTeeth, Resources.MusicPrefix("skin_on_our_teeth.ogg")},
        }.ToImmutableDictionary();

    public static readonly ImmutableDictionary<SFX, string> SFXPaths
        = new Dictionary<SFX, string>
        {
            {SFX.BileBombLand, Resources.SFXPrefix("bile_bomb_land.wav")},
            {SFX.ButtonDefault , Resources.SFXPrefix("button_default.wav")},
            {SFX.ButtonPlay , Resources.SFXPrefix("button_play.wav")},
            {SFX.Death1, Resources.SFXPrefix("death_1.wav")},
            {SFX.Death2, Resources.SFXPrefix("death_2.wav")},
            {SFX.Death3, Resources.SFXPrefix("death_3.wav")},
            {SFX.Explosion1, Resources.SFXPrefix("explosion_1.wav")},
            {SFX.Explosion2, Resources.SFXPrefix("explosion_2.wav")},
            {SFX.Explosion3, Resources.SFXPrefix("explosion_3.wav")},
            {SFX.ExplosionBoomer, Resources.SFXPrefix("explosion_boomer.wav")},
            {SFX.Hit1, Resources.SFXPrefix("hit_1.wav")},
            {SFX.Hit2, Resources.SFXPrefix("hit_2.wav")},
            {SFX.Jump, Resources.SFXPrefix("jump.wav")},
            {SFX.MolotovLand, Resources.SFXPrefix("molotov_land.wav")},
            {SFX.PickupConsumable, Resources.SFXPrefix("pickup_consumable.wav")},
            {SFX.PickupThrowable, Resources.SFXPrefix("pickup_throwable.wav")},
            {SFX.Puke, Resources.SFXPrefix("puke.wav")},
            {SFX.Slam1, Resources.SFXPrefix("slam_1.wav")},
            {SFX.Slam2, Resources.SFXPrefix("slam_2.wav")},
            {SFX.Slam3, Resources.SFXPrefix("slam_3.wav")},
            {SFX.SpecialSpawn, Resources.SFXPrefix("special_spawn.wav")},
            {SFX.SpecialSelect, Resources.SFXPrefix("special_select.wav")},
            {SFX.Spit, Resources.SFXPrefix("spit.wav")},
            {SFX.WeaponLarge1, Resources.SFXPrefix("weapon_large_1.wav")},
            {SFX.WeaponLarge2, Resources.SFXPrefix("weapon_large_2.wav")},
            {SFX.WeaponLarge3, Resources.SFXPrefix("weapon_large_3.wav")},
            {SFX.WeaponSmall, Resources.SFXPrefix("weapon_small.wav")},
            {SFX.WitchAlert, Resources.SFXPrefix("witch_alert.wav")}
        }.ToImmutableDictionary();
    
    public static readonly ImmutableHashSet<SFX> SoundsUnaffectedByCamera
        = new HashSet<SFX>
        {
            SFX.ButtonDefault,
            SFX.ButtonPlay,
            SFX.SpecialSelect
        }.ToImmutableHashSet();     
}