using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI;

public class Subheader
{
    private const int _shadowColor = 0x333333;
    
    private readonly string _text;
    private readonly int _x;
    private readonly int _y;
    private readonly int _color;
    
    public Subheader(string text, int x, int y, int color)
    {
        _text = text;
        _x = x;
        _y = y;
        _color = color;
    }
    
    public void Render(H4D2BitmapCanvas screen)
    {
        screen.DrawLineOfText(H4D2Art.GUI.Text, _text, _x, _y - 1, _shadowColor);
        screen.DrawLineOfText(H4D2Art.GUI.Text, _text, _x, _y, _color);
    }
}