using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2;

public class Game
{
    private readonly Bitmap _screen;
    private Level _level;
    
    public Game(int width, int height)
    {
        _screen = new Bitmap(width, height);
        _level = new Level(width, height);
    }

    public void Update(double elapsedTime)
    {
        _level.Update(elapsedTime);
        if (_level.IsGameOver && _level.CanReset)
        {
            _level = new Level(_level.Width, _level.Height);
        }
    }
    
    public byte[] Render()
    {
        _screen.Clear();
        _level.Render(_screen);
        return _screen.Data;
    }
}