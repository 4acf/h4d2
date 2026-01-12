namespace H4D2.Weapons;

public class WeaponConfig
{
    public required int Damage { get; init; }
    public required double ReloadTimeSeconds { get; init; }
    public required double ShootDelaySeconds { get; init; }
    public required int AmmoPerMagazine { get; init; }
    public required double Spread { get; init; }
    public required int Pellets { get; init; }
    public required int Piercing { get; init; }
}

public static class WeaponConfigs
{
    public static readonly WeaponConfig AutoShotgun = new()
    {
        Damage = 23,
        ReloadTimeSeconds = 4.0,
        ShootDelaySeconds = 1 / 3.33,
        AmmoPerMagazine = 10,
        Spread = 0.5,
        Pellets = 11,
        Piercing = 2
    };

    public static readonly WeaponConfig Deagle = new()
    {
        Damage = 80,
        ReloadTimeSeconds = 1.0,
        ShootDelaySeconds = 1 / 3.33,
        AmmoPerMagazine = 8,
        Spread = 0.05,
        Pellets = 1,
        Piercing = 5
    };

    public static readonly WeaponConfig GrenadeLauncher = new()
    {
        Damage = 400,
        ReloadTimeSeconds = 0.0,
        ShootDelaySeconds = 2.5,
        AmmoPerMagazine = 2,
        Spread = 0.0,
        Pellets = 1,
        Piercing = 0
    };

    public static readonly WeaponConfig HuntingRifle = new()
    {
        Damage = 90,
        ReloadTimeSeconds = 3.13,
        ShootDelaySeconds = 1.0 / 4.0,
        AmmoPerMagazine = 15,
        Spread = 0.0,
        Pellets = 1,
        Piercing = 5
    };

    public static readonly WeaponConfig M16 = new()
    {
        Damage = 33,
        ReloadTimeSeconds = 2.2,
        ShootDelaySeconds = 1.0 / 11.43,
        AmmoPerMagazine = 30,
        Spread = 0.25,
        Pellets = 1,
        Piercing = 2
    };

    public static readonly WeaponConfig PumpShotgun = new()
    {
        Damage = 25,
        ReloadTimeSeconds = 2.0,
        ShootDelaySeconds = 1 / 1.5,
        AmmoPerMagazine = 8,
        Spread = 0.5,
        Pellets = 10,
        Piercing = 3
    };

    public static readonly WeaponConfig Uzi = new()
    {
        Damage = 20,
        ReloadTimeSeconds = 1.8,
        ShootDelaySeconds = 1.0 / 16.0,
        AmmoPerMagazine = 50,
        Spread = 0.3,
        Pellets = 1,
        Piercing = 1
    };

    public static readonly WeaponConfig MegaCoachShotgun = new()
    {
        Damage = 100,
        ReloadTimeSeconds = 0.0,
        ShootDelaySeconds = 1.0 / 16.0,
        AmmoPerMagazine = 100,
        Spread = 0.25,
        Pellets = 15,
        Piercing = 5
    };
}