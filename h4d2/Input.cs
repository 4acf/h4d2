using H4D2.Infrastructure;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace H4D2;

public enum MovementKey
{
    W,
    A,
    S,
    D
} 

public class Input
{
    private readonly RenderWindow _window;

    private Vector2i _mousePositionWindow;
    public ReadonlyPosition MousePositionScreen => 
        new (
            (double)_mousePositionWindow.X / H4D2.ScreenScale, 
            H4D2.ScreenHeight - ((double)_mousePositionWindow.Y / H4D2.ScreenScale)
        );
    
    private readonly HashSet<MovementKey> _pressedMovementKeys;
    public IReadOnlyCollection<MovementKey> PressedMovementKeys => _pressedMovementKeys;
    
    public bool IsMousePressed { get; private set; }
    public bool IsEscPressed { get; private set; } 
    public bool IsNumberPressed { get; private set; }
    public int LastNumberPressed { get; private set; }
    public bool ClickProcessed { get; private set; }

    public Input(RenderWindow window)
    {
        _window = window;
        _pressedMovementKeys = new HashSet<MovementKey>(8);
        Reset();
    }

    public void Reset()
    {
        _pressedMovementKeys.Clear();
        IsMousePressed = false;
        IsEscPressed = false;
        IsNumberPressed = false;
        LastNumberPressed = 0;
        ClickProcessed = false;
    }
    
    public void CaptureRealtimeInputs()
    {
        if (!_window.HasFocus())
            return;

        _mousePositionWindow = Mouse.GetPosition(_window);
        
        if(Keyboard.IsScancodePressed(Keyboard.Scancode.W) 
           || Keyboard.IsScancodePressed(Keyboard.Scancode.Up))
            _pressedMovementKeys.Add(MovementKey.W);
        if(Keyboard.IsScancodePressed(Keyboard.Scancode.A)
           || Keyboard.IsScancodePressed(Keyboard.Scancode.Left))
            _pressedMovementKeys.Add(MovementKey.A);
        if(Keyboard.IsScancodePressed(Keyboard.Scancode.S)
           || Keyboard.IsScancodePressed(Keyboard.Scancode.Down))
            _pressedMovementKeys.Add(MovementKey.S);
        if(Keyboard.IsScancodePressed(Keyboard.Scancode.D)
           || Keyboard.IsScancodePressed(Keyboard.Scancode.Right))
            _pressedMovementKeys.Add(MovementKey.D);
    }

    public void CaptureMouseButtonPress(MouseButtonEventArgs e)
        => IsMousePressed = e.Button == Mouse.Button.Left;
    
    public void CaptureEventKeypress(KeyEventArgs e)
    {
        switch (e.Code)
        {
            case Keyboard.Key.Escape:
                IsEscPressed = true;
                break;
            case >= Keyboard.Key.Num1 and <= Keyboard.Key.Num8:
                IsNumberPressed = true;
                LastNumberPressed = (e.Code - Keyboard.Key.Num1) + 1;
                break;
            default:
                IsNumberPressed = false;
                LastNumberPressed = 0;
                break;
        }
    }

    public void SetClickProcessed() => ClickProcessed = true;
}