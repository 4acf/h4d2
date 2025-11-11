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
    
    public Game(int width, int height)
    {
        _screen = new Bitmap(width, height);
        _shadows = new ShadowBitmap(width, height);
        _collisionManager = new CollisionManager<CollisionGroup>();
        Collisions.Configure(_collisionManager);
        _level = new Level(width, height, _collisionManager);
    }

    public void Update(double elapsedTime)
    {
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
}