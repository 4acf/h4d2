namespace H4D2;

public class Game
{
    private readonly Bitmap _graphics;
    
    public Game(int width, int height)
    {
        _graphics = new Bitmap(width, height);
    }

    public byte[] Render()
    {
        _graphics.Clear();
        
        //temp test code
        const int spriteSize = Art.SpriteSize;
        for (int i = 0; i < Art.Survivors.Length; i++)
        {
            for (int j = 0; j < Art.Survivors[i].Length; j++)
            {
                _graphics.Draw(Art.Survivors[i][j], j * spriteSize, _graphics.Height - (i * spriteSize));
            }
        }
        
        return _graphics.Data;
    }
}