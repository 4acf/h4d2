using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Spawners;
using H4D2.UI;

namespace H4D2;

public class Game
{
    public event EventHandler? ExitGame;

    private const int _mainMenuLevelIndex = 10;
    private const double _cameraMoveSpeed = 100;
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private readonly SaveManager _saveManager;
    private readonly AudioManager _audioManager;
    private readonly UIManager _uiManager;
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
        _uiManager = new UIManager(saveManager, width, height);
        _uiManager.LevelChangeRequested += _InitializeGameLevel;
        _uiManager.MusicVolumeChangeRequested += _OnMusicVolumeChangeRequested;
        _uiManager.SFXVolumeChangeRequested += _OnSFXVolumeChangeRequested;
        _uiManager.ExitRequested += _OnExitRequested;
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
            new SpecialSpawner(_level, config) :
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
        //_HandleInputCommands(input, elapsedTime);
        _level.Update(elapsedTime);
        _uiManager.Update(input);
    }
    
    public byte[] Render()
    {
        _screen.Clear();
        _shadows.Clear();
        _level.Render(_screen, _shadows);
        _specialSpawner?.Render(_screen);
        _uiManager.Render(_screen);
        return _screen.Data;
    }

    private void _HandleInputCommands(Input input, double elapsedTime)
    {
        if (_isInGame && _specialSpawner != null)
        {
            _specialSpawner.UpdatePosition(input.MousePositionScreen, _camera);
        
            if (input.IsNumberPressed)
            {
                _specialSpawner.SelectSpecial(input.LastNumberPressed);
            }

            if (input.IsMousePressed)
                _specialSpawner.Spawn();

            if (input.PressedMovementKeys.Count > 0)
                _HandleCameraMove(input.PressedMovementKeys, elapsedTime);
        }
    }
    
    private void _HandleCameraMove(IReadOnlyCollection<MovementKey> keys, double elapsedTime)
    {
        foreach (MovementKey key in keys)
        {
            switch (key)
            {
                case MovementKey.W:
                    _camera.MoveY((int)(_cameraMoveSpeed * elapsedTime) + 1);
                    break;
                case MovementKey.A:
                    _camera.MoveX((int)(-_cameraMoveSpeed * elapsedTime) - 1);
                    break;
                case MovementKey.S:
                    _camera.MoveY((int)(-_cameraMoveSpeed * elapsedTime) - 1);
                    break;
                case MovementKey.D:
                default:
                    _camera.MoveX((int)(_cameraMoveSpeed * elapsedTime) + 1);
                    break;
            }
        }  
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