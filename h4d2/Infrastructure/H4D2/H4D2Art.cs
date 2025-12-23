namespace H4D2.Infrastructure.H4D2;

public static class H4D2Art
{
    public const int SpriteSize = 16;
    public const int ParticleSize = 8;
    public const int PickupSize = 8;
    public const int ProjectileSize = 8;
    public const int TileSize = 24;
    public const int TileIsoWidth = 24;
    public const int TileIsoHeight = 12;
    public const int TileIsoHalfHeight = 6;
    public const int TileCenterOffset = 19; // -24 + 6, -24 since it draws from top to bottom, 6 is half of height
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

    // to be moved soon
    public static readonly Bitmap Level1 = Art.LoadBitmap($"{_resourcePrefix}levels.level1.png");
    public static readonly Bitmap Level3 = Art.LoadBitmap($"{_resourcePrefix}levels.level3.png");
    public static readonly Bitmap Level4 = Art.LoadBitmap($"{_resourcePrefix}levels.level4.png");
    public static readonly Bitmap Level7 = Art.LoadBitmap($"{_resourcePrefix}levels.level7.png");
    public static readonly Bitmap Level9 = Art.LoadBitmap($"{_resourcePrefix}levels.level9.png");
    public static readonly Bitmap Level11 = Art.LoadBitmap($"{_resourcePrefix}levels.level11.png");
    public static readonly Bitmap Level12 = Art.LoadBitmap($"{_resourcePrefix}levels.level12.png");
    public static readonly Bitmap Level13 = Art.LoadBitmap($"{_resourcePrefix}levels.level13.png");
    public static readonly Bitmap Level14 = Art.LoadBitmap($"{_resourcePrefix}levels.level14.png");
    public static readonly Bitmap Level15 = Art.LoadBitmap($"{_resourcePrefix}levels.level15.png");
    
    /*
    public static readonly Bitmap Level2 = Art.LoadBitmap($"{_resourcePrefix}levels.level2.png");
    public static readonly Bitmap Level5 = Art.LoadBitmap($"{_resourcePrefix}levels.level5.png");
    public static readonly Bitmap Level6 = Art.LoadBitmap($"{_resourcePrefix}levels.level6.png");
    public static readonly Bitmap Level8 = Art.LoadBitmap($"{_resourcePrefix}levels.level8.png");
    public static readonly Bitmap Level10 = Art.LoadBitmap($"{_resourcePrefix}levels.level10.png");
    */
    
    private static readonly Bitmap[][] _tiles = _LoadTiles();
    public static Bitmap[] Floors => _tiles[0];
    public static Bitmap[] Walls => _tiles[1];
    
    public static TextBitmap[] Text => _LoadPixufFont();
    public static Bitmap[] Explosion => _particles[0];
    public static Bitmap[] HealParticle => _particles[1];
    public static Bitmap[] Fire => _particles[2];
    public static Bitmap[] NullParticle => _particles[3];
    public static Bitmap[] BileOverlays => _bileOverlays.SelectMany(x => x).ToArray();
    
    private static Bitmap[][] _LoadSurvivors() => 
        Art.LoadBitmaps($"{_resourcePrefix}survivor.png", SpriteSize, 8, 53);
    private static Bitmap[][] _LoadCommons() => 
        Art.LoadBitmaps($"{_resourcePrefix}common.png", SpriteSize, 9, 23);
    private static Bitmap[][] _LoadUncommons() => 
        Art.LoadBitmaps($"{_resourcePrefix}uncommon.png", SpriteSize, 5, 23);
    private static Bitmap[][] _LoadSpecials() => 
        Art.LoadBitmaps($"{_resourcePrefix}special.png", SpriteSize, 8, 39);
    private static Bitmap[][] _LoadPickups() => 
        Art.LoadBitmaps($"{_resourcePrefix}pickup.png", PickupSize, 2, 3);
    private static Bitmap[][] _LoadProjectiles() => 
        Art.LoadBitmaps($"{_resourcePrefix}projectile.png", ProjectileSize, 4, 4);
    private static Bitmap[][] _LoadParticles() => 
        Art.LoadBitmaps($"{_resourcePrefix}particle.png", ParticleSize, 4, 4);
    private static Bitmap[][] _LoadBileOverlays() => 
        Art.LoadBitmaps($"{_resourcePrefix}bileoverlay.png", SpriteSize, 2, 2);

    private static Bitmap[][] _LoadTiles() =>
        Art.LoadBitmaps($"{_resourcePrefix}levels.tiles.png", TileSize, 2, 3);
    private static TextBitmap[] _LoadPixufFont() =>
        Art.LoadFontBitmaps($"{_resourcePrefix}pixuf.png", Pixuf.Characters, Pixuf.Widths, 7);
}