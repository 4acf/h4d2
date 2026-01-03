using H4D2.GUI;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Spawners.SpecialSpawners;

namespace H4D2;

public class Game
{
    public event EventHandler? ExitGame;

    private const int _mainMenuLevelIndex = 10;
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private readonly SaveManager _saveManager;
    private readonly AudioManager _audioManager;
    private readonly GUIManager _guiManager;
    private readonly Camera _camera;
    private Bitmap _screen = null!;
    private ShadowBitmap _shadows = null!;
    private Level _level = null!;
    private readonly CollisionManager<CollisionGroup> _collisionManager;
    private SpecialSpawner? _specialSpawner;
    private bool _isInGame;
    
    public Game(int width, int height, SaveManager saveManager, AudioManager audioManager)
    {
        _screenWidth = width;
        _screenHeight = height;
        _saveManager = saveManager;
        _audioManager = audioManager;
        _guiManager = new GUIManager(saveManager, width, height);
        _guiManager.LevelChangeRequested += _InitializeGameLevel;
        _guiManager.MusicVolumeChangeRequested += _OnMusicVolumeChangeRequested;
        _guiManager.SFXVolumeChangeRequested += _OnSFXVolumeChangeRequested;
        _guiManager.ExitRequested += _OnExitRequested;
        _collisionManager = new CollisionManager<CollisionGroup>();
        Collisions.Configure(_collisionManager);
        _camera = new Camera(width, height);
        _InitializeLevel(_mainMenuLevelIndex, false);
    }

    private void _InitializeLevel(int level, bool isInGame)
    {
        LevelConfig config = LevelCollection.Levels[level];
        Bitmap levelBitmap = config.Layout;
        _InitializeCamera(levelBitmap);
        _level = new Level(config, _collisionManager, _camera);
        _screen = new Bitmap(_screenWidth, _screenHeight, _camera);
        _shadows = new ShadowBitmap(_screenWidth, _screenHeight, _camera);
        _isInGame = isInGame;
        _specialSpawner = isInGame ? 
            new SpecialSpawner(_level, config, _camera) :
            null;
    }
    
    private SpecialSpawner _InitializeGameLevel(int level)
    {   
        _InitializeLevel(level, true);
        return _specialSpawner!;
    }

    private void _InitializeCamera(Bitmap levelBitmap)
    {
        _camera.ResetOffsets();
        int lowerYBound = H4D2Art.TileCenterOffset - ((levelBitmap.Height / 2) * H4D2Art.TileIsoHeight);
        int upperYOffset = 
            ((H4D2Art.TileIsoHeight - 1)) + 
            (H4D2Art.TileIsoHeight * (levelBitmap.Height - 1)) + 
            _screenHeight;
        _camera.EditBounds(
            -(levelBitmap.Width * H4D2Art.TileSize),
            lowerYBound,
            _screenWidth,
            lowerYBound + upperYOffset
        );
    }
    
    public void Update(Input input, double elapsedTime)
    {
        _level.Update(elapsedTime);
        _guiManager.Update(input);
        if (_isInGame && _specialSpawner != null)
        {
            _specialSpawner.Update(input, elapsedTime);
            _camera.Update(input.PressedMovementKeys, elapsedTime);
        }
    }
    
    public byte[] Render()
    {
        _screen.Clear();
        _shadows.Clear();
        _level.Render(_screen, _shadows);
        _guiManager.Render(_screen);
        return _screen.Data;
    }

    private void _OnMusicVolumeChangeRequested(object? sender, MusicVolumeChangedEventArgs e)
    {
        _audioManager.UpdateMusicVolume(e.MusicVolume);
        _saveManager.SaveNewMusicVolume(e.MusicVolume);
    }

    private void _OnSFXVolumeChangeRequested(object? sender, SFXVolumeChangedEventArgs e)
    {
        _audioManager.UpdateSFXVolume(e.SFXVolume);
        _saveManager.SaveNewSFXVolume(e.SFXVolume);
    }

    private void _OnExitRequested(object? sender, EventArgs e) =>
        ExitGame?.Invoke(this, EventArgs.Empty);
}