namespace H4D2.Infrastructure;

public static class BlendModes
{
    public static byte Multiply(byte a, byte b)
    {
        return (byte)((a * b) / 0xff);
    }

    public static byte Screen(byte a, byte b)
    {
        int ai = 0xff - a;
        int bi = 0xff - b;
        return (byte)(0xff - ((ai * bi) / 0xff));
    }
    
    public static byte HardLight(byte a, byte b)
    {
        if (b < 0x80)
        {
            return (byte)(2 * Multiply(a, b));
        }
        return Screen((byte)(2 * (b - 0x80)), a);
    }
}