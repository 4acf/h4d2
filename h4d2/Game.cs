using H4D2.Entities.Mobs.Zombies.Specials;
using H4D2.Entities.Mobs.Zombies.Specials.Pinners;
using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;
using H4D2.Levels;

namespace H4D2;

public class Game
{
    private readonly Bitmap _screen;
    private readonly ShadowBitmap _shadows;
    private Level _level;
    private readonly CollisionManager<CollisionGroup> _collisionManager;
    
    private int _selectedSpecial;
    
    public Game(int width, int height)
    {
        _selectedSpecial = 1;
        _screen = new Bitmap(width, height);
        _shadows = new ShadowBitmap(width, height);
        _collisionManager = new CollisionManager<CollisionGroup>();
        Collisions.Configure(_collisionManager);
        _level = new Level(width, height, _collisionManager);
    }

    public void Update(Input input, double elapsedTime)
    {
        _HandleInputCommands(input);
        _level.Update(elapsedTime);
        if (_level.IsGameOver && _level.CanReset)
        {
            _level = new Level(_level.Width, _level.Height, _collisionManager);
        }
    }
    
    public byte[] Render()
    {
        _screen.Clear();
        _shadows.Clear();
        _level.Render(_screen, _shadows);
        return _screen.Data;
    }

    private void _HandleInputCommands(Input input)
    {
        if (input.IsNumberPressed)
            _selectedSpecial = input.LastNumberPressed;

        if (!input.IsMousePressed)
            return;
        
        var position = new Position(
            input.MousePositionScreen.X - (H4D2Art.SpriteSize / 2.0),
            input.MousePositionScreen.Y + H4D2Art.SpriteSize
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
}