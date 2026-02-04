using System.Diagnostics;
using H4D2.Infrastructure.H4D2;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace H4D2;

public static class H4D2
{
    public const uint ScreenWidth = 320;
    public const uint ScreenHeight = 240;
    public static uint ScreenScale { get; private set; }
    public const string WindowTitle = "h4d2";
    private static uint _maxScreenScale;
    
    public static void Main()
    {
        var canvas = new RenderTexture(ScreenWidth, ScreenHeight);
        
        var videoMode = VideoMode.DesktopMode;
        _maxScreenScale = videoMode.Height / ScreenHeight;
        ScreenScale = Math.Max(_maxScreenScale - 1, 1);
        uint windowWidth = ScreenWidth * ScreenScale;
        uint windowHeight = ScreenHeight * ScreenScale;
        
        var window = new RenderWindow(
            new VideoMode(windowWidth, windowHeight),
            WindowTitle,
            Styles.None
        );

        Stream iconStream = H4D2Art.GetRandomWindowIcon();
        Image icon = new Image(iconStream);
        window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
        iconStream.Dispose();
        
        window.Closed += (sender, _) =>
        {
            AudioManager.Instance.Close();
            var w = (RenderWindow)sender!;
            w.Close();
        };
        
        window.SetFramerateLimit(60);

        var input = new Input(window);
        window.MouseButtonPressed += (_, e) =>
        {
            input.CaptureMouseButtonPress(e);
        };
        window.KeyPressed += (_, e) =>
        {
            input.CaptureEventKeypress(e);
        };

        AudioManager.Instance.UpdateMusicVolume(SaveManager.Instance.GetMusicVolume());
        AudioManager.Instance.UpdateSFXVolume(SaveManager.Instance.GetSFXVolume());
        
        var game = new Game((int)ScreenWidth, (int)ScreenHeight);
        game.ExitGame += (_, _) =>
        {
            AudioManager.Instance.Close();
            window.Close();
        };
        
        var scale = new Vector2f(ScreenScale, ScreenScale);
        var stopwatch = new Stopwatch();

        var renderState = new RenderStates(BlendMode.None);
        
        while (window.IsOpen)
        {
            double elapsedTime = stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();
            
            input.Reset();
            window.DispatchEvents();
            input.CaptureRealtimeInputs();
            
            game.Update(input, elapsedTime);
            
            canvas.Clear();
            Texture texture = canvas.Texture;
            texture.Update(game.Render());
            canvas.Display();
            Sprite sprite = new Sprite(canvas.Texture);
            sprite.Scale = scale;
            window.Draw(sprite, renderState);
            window.Display();
        }
    }
}