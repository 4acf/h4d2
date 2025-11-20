namespace H4D2.Infrastructure.H4D2;

public static class H4D2Art
{
    public const int SpriteSize = 16;
    public const int ParticleSize = 8;
    public const int PickupSize = 8;
    public const int ProjectileSize = 8;
    public const int ShadowColor = 0x0;
    public const double ShadowBlend = 0.9;
    private const string _resourcePrefix = "h4d2.Resources.";
    
    public static readonly Bitmap[][] Survivors = _LoadSurvivors();
    public static readonly Bitmap[][] Commons = _LoadCommons();
    public static readonly Bitmap[][] Uncommons = _LoadUncommons();
    public static readonly Bitmap[][] Specials = _LoadSpecials();
    public static readonly Bitmap[][] Pickups = _LoadPickups();
    public static readonly Bitmap[][] Projectiles = _LoadProjectiles();
    private static readonly Bitmap[][] _particles = _LoadParticles();
    private static readonly Bitmap[][] _bileOverlays = _LoadBileOverlays();
    
    public static Bitmap[] Explosion => _particles[0];
    public static Bitmap[] HealParticle => _particles[1];
    public static Bitmap[] Fire => _particles[2];
    public static Bitmap[] NullParticle => _particles[3];
    public static Bitmap[] BileOverlays => _bileOverlays.SelectMany(x => x).ToArray();
    
    private static Bitmap[][] _LoadSurvivors() => 
        Art.LoadBitmaps($"{_resourcePrefix}survivor.png", SpriteSize, 8, 34);
    private static Bitmap[][] _LoadCommons() => 
        Art.LoadBitmaps($"{_resourcePrefix}common.png", SpriteSize, 9, 23);
    private static Bitmap[][] _LoadUncommons() => 
        Art.LoadBitmaps($"{_resourcePrefix}uncommon.png", SpriteSize, 5, 23);
    private static Bitmap[][] _LoadSpecials() => 
        Art.LoadBitmaps($"{_resourcePrefix}special.png", SpriteSize, 8, 29);
    private static Bitmap[][] _LoadPickups() => 
        Art.LoadBitmaps($"{_resourcePrefix}pickup.png", PickupSize, 2, 3);
    private static Bitmap[][] _LoadProjectiles() => 
        Art.LoadBitmaps($"{_resourcePrefix}projectile.png", ProjectileSize, 4, 4);
    private static Bitmap[][] _LoadParticles() => 
        Art.LoadBitmaps($"{_resourcePrefix}particle.png", ParticleSize, 4, 4);
    private static Bitmap[][] _LoadBileOverlays() => 
        Art.LoadBitmaps($"{_resourcePrefix}bileoverlay.png", SpriteSize, 2, 2);
}