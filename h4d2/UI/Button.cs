using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.UI;

public enum ButtonType
{
    Play,
    Settings,
    Exit,
    MainMenu,
    Resume,
    Levels,
    Forward,
    Backward,
    Spawner
}

public class Button
{
    public event EventHandler? Clicked;
    
    private readonly ButtonType _type;
    private readonly int _x;
    private readonly int _y;
    private readonly int _width;
    private readonly int _height;
    private readonly bool _xFlip;
    private bool _isMouseOver;
    
    public Button(ButtonType type, int x, int y)
    {
        _type = type;
        _x = x;
        _y = y;
        _xFlip = type == ButtonType.Backward;
        _isMouseOver = false;

        if
        (
            type == ButtonType.Forward ||
            type == ButtonType.Backward ||
            type == ButtonType.Spawner
        )
        {
            _width = H4D2Art.SmallButtonSize;
            _height = H4D2Art.SmallButtonSize;
        }
        else
        {
            _width = H4D2Art.LargeButtonWidth;
            _height = H4D2Art.LargeButtonHeight;
        }
    }

    public void Update(Input input)
    {
        _UpdateMouseOverState(input.MousePositionScreen);
        if (_isMouseOver && input.IsMousePressed)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void Render(Bitmap screen)
    {
        Bitmap bitmap = _ResolveBitmap();
        screen.DrawAbsolute(bitmap, _x, _y, _xFlip);
    }

    private void _UpdateMouseOverState(ReadonlyPosition mousePosition)
    {
        if
        (
            _x <= mousePosition.X &&
            mousePosition.X <= _x + _width &&
            _y - _height <= mousePosition.Y &&
            mousePosition.Y <= _y
        )
            _isMouseOver = true;
        else
            _isMouseOver = false;
    }
    
    private Bitmap _ResolveBitmap()
    {
        int hoverState = _isMouseOver ? 1 : 0;
        return _type switch
        {
            ButtonType.Play => H4D2Art.Buttons.Play[hoverState],
            ButtonType.Settings => H4D2Art.Buttons.Settings[hoverState],
            ButtonType.Exit => H4D2Art.Buttons.Exit[hoverState],
            ButtonType.MainMenu => H4D2Art.Buttons.MainMenu[hoverState],
            ButtonType.Resume => H4D2Art.Buttons.Resume[hoverState],
            ButtonType.Levels => H4D2Art.Buttons.Levels[hoverState],
            ButtonType.Forward or ButtonType.Backward => H4D2Art.Buttons.Navigation[hoverState],
            ButtonType.Spawner => H4D2Art.Buttons.Spawner[hoverState],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}