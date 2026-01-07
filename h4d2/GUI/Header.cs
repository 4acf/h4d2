using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI;

public class Header
{
    private const int _shadowColor = 0x333333;
    
    private string _text;
    private readonly int _x;
    private readonly int _y;
    private readonly int _color;
    
    public Header(string text, int x, int y, int color)
    {
        _text = text;
        _x = x;
        _y = y;
        _color = color;
    }

    public void UpdateText(string text)
    {
        _text = text;
    }
    
    public void Render(H4D2BitmapCanvas screen)
    {
        screen.DrawTextHeader(H4D2Art.GUI.Text, _text, _x, _y - 2, _shadowColor);
        screen.DrawTextHeader(H4D2Art.GUI.Text, _text, _x, _y, _color);
    }
}