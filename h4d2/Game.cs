using System.Diagnostics;
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
    private readonly GUIManager _guiManager;
    private readonly Camera _camera;
    private H4D2BitmapCanvas _screen = null!;
    private ShadowBitmap _shadows = null!;
    private Level _level = null!;
    private CheatCode _cheatCode = null!;
    private readonly CollisionManager<CollisionGroup> _collisionManager;
    private SpecialSpawner? _specialSpawner;
    private bool _isInGame;
    private bool _isPaused;
    private Stopwatch? _stopwatch;
    
    public Game(int width, int height)
    {
        _screenWidth = width;
        _screenHeight = height;
        _guiManager = new GUIManager(width, height);
        _guiManager.LevelChangeRequested += _InitializeGameLevel;
        _guiManager.MusicVolumeChangeRequested += _OnMusicVolumeChangeRequested;
        _guiManager.SFXVolumeChangeRequested += _OnSFXVolumeChangeRequested;
        _guiManager.PauseRequested += _OnPauseToggleRequested;
        _guiManager.UnpauseRequested += _OnPauseToggleRequested;
        _guiManager.ReloadMainMenuRequested += _OnReloadMainMenuRequested;
        _guiManager.ExitRequested += _OnExitRequested;
        _collisionManager = new CollisionManager<CollisionGroup>();
        Collisions.Configure(_collisionManager);
        _camera = new Camera(width, height);
        AudioManager.Instance.SetCamera(_camera);
        _InitializeLevel(_mainMenuLevelIndex, false);
    }

    private void _InitializeLevel(int level, bool isInGame)
    {
        if(!isInGame)
            AudioManager.Instance.PlayMusic(Track.TheParish);
        LevelConfig config = LevelCollection.Levels[level];
        Bitmap levelBitmap = config.Layout;
        _InitializeCamera(levelBitmap);
        _level = new Level(config, _collisionManager, _camera);
        _level.GameOver += isInGame ? 
            _OnGameOver :
            _OnMainMenuGameOver;
        _screen = new H4D2BitmapCanvas(_screenWidth, _screenHeight, _camera);
        _shadows = new ShadowBitmap(_screenWidth, _screenHeight, _camera);
        _cheatCode = new CheatCode(_level);
        _isInGame = isInGame;
        AudioManager.Instance.SetInGameState(isInGame);
        _isPaused = false;
        _specialSpawner = isInGame ? 
            new SpecialSpawner(_level, config, _camera) :
            null;
    }
    
    private SpecialSpawner _InitializeGameLevel(int level)
    {   
        LevelConfig config = LevelCollection.Levels[level];
        AudioManager.Instance.PlayMusic(config.MainTheme);
        _InitializeLevel(level, true);
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
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
        if(!_isInGame || !_isPaused)
            _level.Update(elapsedTime);
        
        _guiManager.Update(input, elapsedTime);
        
        if (_isInGame && !_isPaused && !_level.IsGameOver && _specialSpawner != null)
        {
            _specialSpawner.Update(input, elapsedTime);
            _camera.Update(input.PressedMovementKeys, elapsedTime);
            _cheatCode.Update(input);
        }
    }
    
    public byte[] Render()
    {
        _screen.Clear();
        _shadows.Clear();
        _level.Render(_camera, _screen, _shadows);
        _guiManager.Render(_screen);
        return _screen.Data;
    }

    private void _OnPauseToggleRequested(object? sender, EventArgs e)
    {
        if (!_isInGame)
            return;
        _isPaused = !_isPaused;
        if(_isPaused)
            _stopwatch?.Stop();
        else
            _stopwatch?.Start();
    }
    
    private static void _OnMusicVolumeChangeRequested(object? sender, MusicVolumeChangedEventArgs e)
    {
        AudioManager.Instance.UpdateMusicVolume(e.MusicVolume);
        SaveManager.Instance.SaveNewMusicVolume(e.MusicVolume);
    }

    private static void _OnSFXVolumeChangeRequested(object? sender, SFXVolumeChangedEventArgs e)
    {
        AudioManager.Instance.UpdateSFXVolume(e.SFXVolume);
        SaveManager.Instance.SaveNewSFXVolume(e.SFXVolume);
    }

    private void _OnReloadMainMenuRequested(object? sender, EventArgs e) =>
        _InitializeLevel(_mainMenuLevelIndex, false);

    private void _OnExitRequested(object? sender, EventArgs e) =>
        ExitGame?.Invoke(this, EventArgs.Empty);
    
    private void _OnGameOver(object? sender, EventArgs e) 
    {
        double totalElapsedTime = _stopwatch != null ? 
            _stopwatch.Elapsed.TotalSeconds : 0;
        AudioManager.Instance.PlayMusic(Track.TheMonstersWithin);
        SaveManager.Instance.SaveNewLevelRecord(_level.ID, totalElapsedTime);
        _guiManager.ForceNavigateToLevelCompleteMenu(_level.ID, totalElapsedTime);
        _stopwatch = null;
    }

    private void _OnMainMenuGameOver(object? sender, EventArgs e) =>
        _InitializeLevel(_mainMenuLevelIndex, false);

}