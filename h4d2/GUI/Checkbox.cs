using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI;

public class Checkbox
{
    public event EventHandler? ValueUpdated;

    public bool IsEnabled { get; private set; }

    public const int Size = 9;
    
    private readonly int _x;
    private readonly int _y;
    private bool _isMouseOver;
    
    public Checkbox(int x, int y, bool isEnabled)
    {
        _x = x;
        _y = y;
        _isMouseOver = false;
        IsEnabled = isEnabled;
    }

    public void Update(Input input)
    {
        _UpdateMouseOverState(input.MousePositionScreen);
        if (_isMouseOver && input.IsMousePressed)
        {
            IsEnabled = !IsEnabled;
            ValueUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Render(H4D2BitmapCanvas screen)
    {
        int color = IsEnabled ? 0xffffff : 0x202020;
        screen.FillAbsolute
        (
            _x, 
            _y - Size - 1,
            _x + Size - 1,
            _y - Size - 1,
            0x0
        );
        screen.FillAbsolute
        (
            _x, 
            _y - Size,
            _x + Size - 1,
            _y,
            color
        );
    }
    
    private void _UpdateMouseOverState(ReadonlyPosition mousePosition)
    {
        if
        (
            _x <= mousePosition.X &&
            mousePosition.X <= _x + Size &&
            _y - Size <= mousePosition.Y &&
            mousePosition.Y <= _y
        )
            _isMouseOver = true;
        else
            _isMouseOver = false;
    }
}