using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Zombies.Specials.Pinners;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2;

public class Game
{
    private const double _cameraMoveSpeed = 100; 
    
    private readonly Camera _camera;
    private readonly Bitmap _screen;
    private readonly ShadowBitmap _shadows;
    private Level _level;
    private readonly CollisionManager<CollisionGroup> _collisionManager;
    
    private int _selectedSpecial;
    
    public Game(int width, int height)
    {
        _selectedSpecial = 1;
        _collisionManager = new CollisionManager<CollisionGroup>();
        Collisions.Configure(_collisionManager);
        Bitmap levelBitmap = H4D2Art.Level1;
        _camera = new Camera();
        int lowerYBound = H4D2Art.TileCenterOffset - ((levelBitmap.Height / 2) * H4D2Art.TileIsoHeight);
        int upperYOffset = 
            (1 * (H4D2Art.TileIsoHeight - 1)) + (H4D2Art.TileIsoHeight * (levelBitmap.Height - 1)) + height;
        _camera.InitBounds(
            -(levelBitmap.Width * H4D2Art.TileSize),
            lowerYBound,
            width,
            lowerYBound + upperYOffset
        );
        _level = new Level(levelBitmap, _collisionManager, _camera);
        _screen = new Bitmap(width, height, _camera);
        _shadows = new ShadowBitmap(width, height, _camera);
    }

    public void Update(Input input, double elapsedTime)
    {
        _HandleInputCommands(input, elapsedTime);
        _level.Update(elapsedTime);
        /*
        if (_level.IsGameOver && _level.CanReset)
        {
            _level = new Level(_level.Width, _level.Height, _collisionManager);
        }
        */
    }
    
    public byte[] Render()
    {
        _screen.Clear();
        _shadows.Clear();
        _level.Render(_screen, _shadows);
        return _screen.Data;
    }

    private void _HandleInputCommands(Input input, double elapsedTime)
    {
        if (input.IsNumberPressed)
            _selectedSpecial = input.LastNumberPressed;

        if (input.IsMousePressed)
            _HandleMousePressed(input.MousePositionScreen);

        if (input.PressedMovementKeys.Count > 0)
             _HandleCameraMove(input.PressedMovementKeys, elapsedTime);
    }

    private void _HandleMousePressed(Position mousePosition)
    {
        var position = new Position(
            mousePosition.X - (H4D2Art.SpriteSize / 2.0),
            mousePosition.Y + H4D2Art.SpriteSize
        );
        Special special = _selectedSpecial switch
        {
            1 => new Hunter(_level, position),
            2 => new Boomer(_level, position),
            3 => new Smoker(_level, position),
            4 => new Charger(_level, position),
            5 => new Jockey(_level, position),
            6 => new Spitter(_level, position),
            7 => new Tank(_level, position),
            8 => new Witch(_level, position),
            _ => new Tank(_level, position)
        };
        _level.AddSpecial(special);
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
}