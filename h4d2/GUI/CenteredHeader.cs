using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI;

public class CenteredHeader
{
    private const int _shadowColor = 0x333333;
    
    private string _text;
    private readonly int _y;
    private readonly int _color;
    
    public CenteredHeader(string text, int y, int color)
    {
        _text = text;
        _y = y;
        _color = color;
    }

    public void UpdateText(string text)
    {
        _text = text;
    }
    
    public void Render(Bitmap screen)
    {
        screen.DrawCenteredTextHeader(H4D2Art.Text, _text, _y - 2, _shadowColor);
        screen.DrawCenteredTextHeader(H4D2Art.Text, _text, _y, _color);
    }
}