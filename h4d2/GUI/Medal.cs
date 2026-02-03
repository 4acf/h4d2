using H4D2.Infrastructure;
using H4D2.Infrastructure.H4D2;

namespace H4D2.GUI;

public class Medal
{
    private readonly Bitmap _bitmap;
    private readonly int _x;
    private readonly int _y;

    public Medal(double time, int x, int y)
    {
        _bitmap = time switch
        {
            < 60.0 => H4D2Art.GUI.Medals.Platinum,
            < 120.0 => H4D2Art.GUI.Medals.Gold,
            < 180.0 => H4D2Art.GUI.Medals.Silver,
            _ => H4D2Art.GUI.Medals.Bronze
        };

        _x = x;
        _y = y;
    }

    public void Render(H4D2BitmapCanvas screen)
    {
        screen.DrawAbsolute(_bitmap, _x, _y);
    }
}