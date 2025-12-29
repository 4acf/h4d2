using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;
using H4D2.Spawners;
using H4D2.UI;

namespace H4D2;

public class Game
{
    public event EventHandler? ExitGame;
    
    private const double _cameraMoveSpeed = 100;
    private const double _defaultVolume = 1.0;
    
    private double _musicVolume;
    private double _sfxVolume;
    
    private readonly UIManager _uiManager;
    private readonly Camera _camera;
    private readonly Bitmap _screen;
    private readonly ShadowBitmap _shadows;
    private Level _level;
    private readonly CollisionManager<CollisionGroup> _collisionManager;
    private SpecialSpawner? _specialSpawner;
    private bool _isInGame;
    
    public Game(int width, int height)
    {
        _uiManager = new UIManager(width, height);
        _uiManager.ExitRequested += _OnExitRequested;

        _musicVolume = _defaultVolume;
        _sfxVolume = _defaultVolume;
        
        _collisionManager = new CollisionManager<CollisionGroup>();
        Collisions.Configure(_collisionManager);
        Bitmap levelBitmap = H4D2Art.Level10;
        
        _camera = new Camera(width, height);
        int lowerYBound = H4D2Art.TileCenterOffset - ((levelBitmap.Height / 2) * H4D2Art.TileIsoHeight);
        int upperYOffset = 
            ((H4D2Art.TileIsoHeight - 1)) + 
            (H4D2Art.TileIsoHeight * (levelBitmap.Height - 1)) + 
            height;
        _camera.InitBounds(
            -(levelBitmap.Width * H4D2Art.TileSize),
            lowerYBound,
            width,
            lowerYBound + upperYOffset
        );
        
        _level = new Level(levelBitmap, _collisionManager, _camera);
        _screen = new Bitmap(width, height, _camera);
        _shadows = new ShadowBitmap(width, height, _camera);
        _specialSpawner = null;
        _isInGame = false;
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
    
    private void _OnExitRequested(object? sender, EventArgs e) =>
        ExitGame?.Invoke(this, EventArgs.Empty);
}