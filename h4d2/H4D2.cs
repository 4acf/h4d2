using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace H4D2;

public static class H4D2
{
    public const uint ScreenWidth = 320;
    public const uint ScreenHeight = 240;
    public const uint ScreenScale = 3;
    public const uint WindowWidth = ScreenWidth * ScreenScale;
    public const uint WindowHeight = ScreenHeight * ScreenScale;
    public const string WindowTitle = "h4d2";
    
    public static void Main()
    {
        var canvas = new RenderTexture(ScreenWidth, ScreenHeight);

        var window = new RenderWindow(
            new VideoMode(WindowWidth, WindowHeight),
            WindowTitle,
            Styles.Close
        );
        window.Closed += (s, _) =>
        {
            var w = (RenderWindow)s!;
            w.Close();
        };
        window.SetFramerateLimit(60);
        
        var game = new Game((int)ScreenWidth, (int)ScreenHeight);
        var scale = new Vector2f(ScreenScale, ScreenScale);
        var stopwatch = new Stopwatch();
        
        while (window.IsOpen)
        {
            stopwatch.Stop();
            double elapsedTime = stopwatch.Elapsed.TotalSeconds;
            stopwatch = Stopwatch.StartNew();
            
            window.DispatchEvents();
            
            game.Update(elapsedTime);
            
            canvas.Clear();
            Texture texture = canvas.Texture;
            texture.Update(game.Render());
            canvas.Display();
            Sprite sprite = new Sprite(canvas.Texture);
            sprite.Scale = scale;
            window.Draw(sprite);
            window.Display();
        }
    }
}