using H4D2.Infrastructure;
using H4D2.Levels;

namespace H4D2;

public class Game
{
    private readonly Bitmap _screen;
    private readonly Level _level;
    
    public Game(int width, int height)
    {
        _screen = new Bitmap(width, height);
        _level = new Level();
    }

    public void Update(double elapsedTime)
    {
        _level.UpdateEntities(elapsedTime);
    }
    
    public byte[] Render()
    {
        _screen.Clear();
        _level.RenderBackground(_screen);
        _level.RenderEntities(_screen);
        return _screen.Data;
    }
}