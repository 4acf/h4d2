namespace H4D2.Infrastructure;

public readonly struct RGB
{
    public readonly byte Red;
    public readonly byte Green;
    public readonly byte Blue;

    public RGB()
    {
        Red = 0;
        Green = 0;
        Blue = 0;
    }
    
    public RGB(int color)
    {
        Red = (byte)(color >> 16 & 0xff);
        Green = (byte)(color >> 8 & 0xff);
        Blue = (byte)(color & 0xff);
    }

    public static int ToInt(byte r, byte g, byte b)
    {
        int result = r;
        result = (result << 8) | g;
        result = (result << 8) | b;
        return result;
    }
}