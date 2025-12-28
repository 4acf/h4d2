using SkiaSharp;

namespace H4D2.Infrastructure;

public class Bitmap
{
    public readonly byte[] Data;
    public readonly int Width;
    public readonly int Height;

    private readonly Camera? _camera;
    
    public Bitmap(int width, int height, Camera? camera = null)
    {
        int numBytes = width * height * 4;
        Data = new byte[numBytes];
        Width = width;
        Height = height;
        _camera = camera;
    }

    public Bitmap(SKBitmap bitmap)
    {
        Width = bitmap.Width;
        Height = bitmap.Height;
        int numBytes = Width * Height * 4;
        Data = new byte[numBytes];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SKColor color = bitmap.GetPixel(x, y);
                int index = (y * Width * 4) + (x * 4);
                Data[index] = color.Red;
                Data[index + 1] = color.Green;
                Data[index + 2] = color.Blue;
                Data[index + 3] = color.Alpha;
            }
        }
    }
    
    public Bitmap(SKBitmap bitmap, int spriteSize, int row, int col)
    {
        int numBytes = spriteSize * spriteSize * 4;
        Data = new byte[numBytes];

        for (int i = 0; i < spriteSize; i++)
        {
            for (int j = 0; j < spriteSize; j++)
            {
                int x = (col * spriteSize) + j;
                int y = (row * spriteSize) + i;
                SKColor color = bitmap.GetPixel(x, y);

                int index = (i * spriteSize * 4) + (j * 4);
                Data[index] = color.Red;
                Data[index + 1] = color.Green;
                Data[index + 2] = color.Blue;
                Data[index + 3] = color.Alpha;
            }
        }
        
        Width = spriteSize;
        Height = spriteSize;
    }
    
    public Bitmap(SKBitmap bitmap, int spriteWidth, int spriteHeight, int row, int col)
    {
        int numBytes = spriteWidth * spriteHeight * 4;
        Data = new byte[numBytes];

        for (int i = 0; i < spriteHeight; i++)
        {
            for (int j = 0; j < spriteWidth; j++)
            {
                int x = (col * spriteWidth) + j;
                int y = (row * spriteHeight) + i;
                SKColor color = bitmap.GetPixel(x, y);

                int index = (i * spriteWidth * 4) + (j * 4);
                Data[index] = color.Red;
                Data[index + 1] = color.Green;
                Data[index + 2] = color.Blue;
                Data[index + 3] = color.Alpha;
            }
        }
        
        Width = spriteWidth;
        Height = spriteHeight;
    }

    public int ColorAt(int x, int y)
    {
        int result = 0x0;
        int index = _GetBytespaceIndex(Width, x, y);
        if (IsOutOfBounds(index))
            return result;
        byte r = Data[index];
        byte g = Data[index + 1];
        byte b = Data[index + 2];
        result = r;
        result = (result << 8) | g;
        result = (result << 8) | b;
        return result;
    }
    
    public void Clear(int? color = 0x0)
    {
        byte r = 0, g = 0, b = 0;
        
        if (color != null)
        {
            r = (byte)(color >> 16 & 0xff);
            g = (byte)(color >> 8 & 0xff);
            b = (byte)(color & 0xff);
        }
        
        for (int i = 0; i < Data.Length; i += 4)
        {
            Data[i] = r;
            Data[i + 1] = g;
            Data[i + 2] = b;
        }
    }

    public void DrawAbsolute(Bitmap bitmap, int x, int y, bool flip = false)
    {
        for (int i = 0; i < bitmap.Height; i++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                int k = j;
                if (flip)
                {
                    k = bitmap.Width - j - 1;
                }
                
                int parentIndex = _GetBytespaceIndex(Width, x + j, y - i - 1);
                int childIndex = _GetBytespaceIndex(bitmap.Width, k, i);
                
                if (IsOutOfBounds(parentIndex) || bitmap.IsOutOfBounds(childIndex)) continue;
                if (bitmap.Data[childIndex + 3] == 0) continue;

                int expectedY = y - i - 1;
                int actualY = parentIndex / (Width * 4);
                if (expectedY != actualY) continue;
                
                Data[parentIndex] = bitmap.Data[childIndex];
                Data[parentIndex + 1] = bitmap.Data[childIndex + 1];
                Data[parentIndex + 2] = bitmap.Data[childIndex + 2];
                Data[parentIndex + 3] = bitmap.Data[childIndex + 3];
            }
        }
    }
    
    public void Draw(Bitmap bitmap, int x, int y, bool flip = false)
    {
        if (_camera != null)
        {
            x += _camera.XOffset;
            y += _camera.YOffset;
        }
        
        DrawAbsolute(bitmap, x, y, flip);
    }

    public void DrawLineOfText(TextBitmap[] textBitmaps, string text, int x, int y, int color = 0x0)
    {
        int startingX = x;
        for (int i = 0; i < text.Length; i++)
        {
            TextBitmap letterBitmap = textBitmaps[text[i] - ' '];
            _DrawLetter(letterBitmap, startingX, y, color);
            startingX += letterBitmap.Width;
        }
    }

    public void DrawTextHeader(TextBitmap[] textBitmaps, string text, int x, int y, int color = 0x0)
    {
        int startingX = x;
        for (int i = 0; i < text.Length; i++)
        {
            TextBitmap letterBitmap = textBitmaps[text[i] - ' '];
            _DrawDoubleSizeLetter(letterBitmap, startingX, y, color);
            startingX += letterBitmap.Width * 2;
        }
    }

    public void DrawCenteredTextHeader(TextBitmap[] textBitmaps, string text, int y, int color = 0x0)
    {
        int width = 0;
        for (int i = 0; i < text.Length; i++)
        {
            TextBitmap letterBitmap = textBitmaps[text[i] - ' '];
            width += letterBitmap.Width * 2;
        }

        width += 2;
        int x = (Width / 2) - (width / 2);
        DrawTextHeader(textBitmaps, text, x, y, color);
    }
    
    private void _DrawLetter(TextBitmap letterBitmap, int x, int y, int color)
    {
        byte r = (byte)(color >> 16 & 0xff);
        byte g = (byte)(color >> 8 & 0xff);
        byte b = (byte)(color & 0xff);
        
        for (int i = 0; i < letterBitmap.Height; i++)
        {
            for (int j = 0; j < letterBitmap.Width; j++)
            {
                int parentIndex = _GetBytespaceIndex(Width, x + j, y - i - 1);
                int childIndex = letterBitmap.GetBytespaceIndex(j, i);
                
                if (IsOutOfBounds(parentIndex) || letterBitmap.IsOutOfBounds(childIndex)) continue;
                if (!letterBitmap.Data[childIndex]) continue;

                int expectedY = y - i - 1;
                int actualY = parentIndex / (Width * 4);
                if (expectedY != actualY) continue;
                
                Data[parentIndex] = r;
                Data[parentIndex + 1] = g;
                Data[parentIndex + 2] = b;
                Data[parentIndex + 3] = 0xff;
            }
        }
    }

    private void _DrawDoubleSizeLetter(TextBitmap letterBitmap, int x, int y, int color)
    {
        byte r = (byte)(color >> 16 & 0xff);
        byte g = (byte)(color >> 8 & 0xff);
        byte b = (byte)(color & 0xff);
        
        for (int i = 0; i < letterBitmap.Height; i++)
        {
            for (int j = 0; j < letterBitmap.Width; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    int xOffs = k % 2;
                    int yOffs = -(k / 2);
                    
                    int parentIndex = _GetBytespaceIndex(Width, x + (j * 2) + xOffs, y - (i * 2) - 1 + yOffs);
                    int childIndex = letterBitmap.GetBytespaceIndex(j, i);
                
                    if (IsOutOfBounds(parentIndex) || letterBitmap.IsOutOfBounds(childIndex)) continue;
                    if (!letterBitmap.Data[childIndex]) continue;

                    int expectedY = y - (i * 2) - 1 + yOffs;
                    int actualY = parentIndex / (Width * 4);
                    if (expectedY != actualY) continue;
                
                    Data[parentIndex] = r;
                    Data[parentIndex + 1] = g;
                    Data[parentIndex + 2] = b;
                    Data[parentIndex + 3] = 0xff;   
                }
            }
        }
    }

    public void DrawInvalidSpecial(Bitmap specialBitmap, int x, int y)
    {
        const double blend = 0.5;
        
        if (_camera != null)
        {
            x += _camera.XOffset;
            y += _camera.YOffset;
        }
        
        for (int i = 0; i < specialBitmap.Height; i++)
        {
            for (int j = 0; j < specialBitmap.Width; j++)
            {
                int parentIndex = _GetBytespaceIndex(Width, x + j, y - i - 1);
                int specialIndex = _GetBytespaceIndex(specialBitmap.Width, j, i);
                
                if (IsOutOfBounds(parentIndex) || specialBitmap.IsOutOfBounds(specialIndex)) continue;
                if (specialBitmap.Data[specialIndex + 3] == 0) continue;

                int expectedY = y - i - 1;
                int actualY = parentIndex / (Width * 4);
                if (expectedY != actualY) continue;
                
                byte r = BlendModes.HardLight(specialBitmap.Data[specialIndex], 0xaa);
                byte g = BlendModes.HardLight(specialBitmap.Data[specialIndex + 1], 0x00);
                byte b = BlendModes.HardLight(specialBitmap.Data[specialIndex + 2], 0x00);

                Data[parentIndex] = MathHelpers.ByteLerp(specialBitmap.Data[specialIndex], r, blend);
                Data[parentIndex + 1] = MathHelpers.ByteLerp(specialBitmap.Data[specialIndex + 1], g, blend);
                Data[parentIndex + 2] = MathHelpers.ByteLerp(specialBitmap.Data[specialIndex + 2], b, blend);
                Data[parentIndex + 3] = specialBitmap.Data[specialIndex + 3];
            }
        }
    }
    
    public void DrawBiledCharacter(Bitmap characterBitmap, Bitmap bileBitmap, int x, int y, bool flip = false)
    {
        if(characterBitmap.Width != bileBitmap.Width || characterBitmap.Height != bileBitmap.Height)
            throw new ArgumentException("Character and bile bitmaps must have the same width and height.");
     
        if (_camera != null)
        {
            x += _camera.XOffset;
            y += _camera.YOffset;
        }
        
        const double blend = 0.3;
        
        for (int i = 0; i < characterBitmap.Height; i++)
        {
            for (int j = 0; j < characterBitmap.Width; j++)
            {
                int k = j;
                if (flip)
                {
                    k = characterBitmap.Width - j - 1;
                }
                
                int parentIndex = _GetBytespaceIndex(Width, x + j, y - i - 1);
                int characterIndex = _GetBytespaceIndex(characterBitmap.Width, k, i);
                int bileIndex = _GetBytespaceIndex(bileBitmap.Width, k, i);
                
                if (IsOutOfBounds(parentIndex) || characterBitmap.IsOutOfBounds(characterIndex)) continue;
                if (characterBitmap.Data[characterIndex + 3] == 0) continue;

                int expectedY = y - i - 1;
                int actualY = parentIndex / (Width * 4);
                if (expectedY != actualY) continue;
                
                byte r = BlendModes.HardLight(characterBitmap.Data[characterIndex], bileBitmap.Data[bileIndex]);
                byte g = BlendModes.HardLight(characterBitmap.Data[characterIndex + 1], bileBitmap.Data[bileIndex + 1]);
                byte b = BlendModes.HardLight(characterBitmap.Data[characterIndex + 2], bileBitmap.Data[bileIndex + 2]);

                Data[parentIndex] = MathHelpers.ByteLerp(characterBitmap.Data[characterIndex], r, blend);
                Data[parentIndex + 1] = MathHelpers.ByteLerp(characterBitmap.Data[characterIndex + 1], g, blend);
                Data[parentIndex + 2] = MathHelpers.ByteLerp(characterBitmap.Data[characterIndex + 2], b, blend);
                Data[parentIndex + 3] = characterBitmap.Data[characterIndex + 3];
            }
        }
    }
    
    public void DrawShadows(ShadowBitmap shadows, byte shadowColor, double shadowBlend)
    {
        if (Width != shadows.Width || Height != shadows.Height)
            throw new ArgumentException("Shadow bitmap dimensions do not match base bitmap dimensions.");

        for (int i = 0; i < Data.Length; i += 4)
        {
            int j = i / 4;
            bool set = shadows.Data[j];
            if (!set) continue;
            Data[i] = MathHelpers.ByteLerp(Data[i], shadowColor, shadowBlend);
            Data[i + 1] = MathHelpers.ByteLerp(Data[i + 1], shadowColor, shadowBlend);
            Data[i + 2] = MathHelpers.ByteLerp(Data[i + 2], shadowColor, shadowBlend);
            Data[i + 3] = 0xff;
        }
    }

    public void FillAbsolute(int x0, int y0, int x1, int y1, int color)
    {
        FillBlendAbsolute(x0, y0, x1, y1, color, 0);
    }

    public void FillBlendAbsolute(int x0, int y0, int x1, int y1, int color, double blend)
    {
        byte r = (byte)(color >> 16 & 0xff);
        byte g = (byte)(color >> 8 & 0xff);
        byte b = (byte)(color & 0xff);

        for (int i = y0; i <= y1; i++)
        {
            for (int j = x0; j <= x1; j++)
            {
                int index = _GetBytespaceIndex(Width, j, i);
                if (IsOutOfBounds(index)) continue;
                
                int expectedY = y0 + (i - y0);
                int actualY = index / (Width * 4);
                if (expectedY != actualY) continue;
                
                Data[index] = MathHelpers.ByteLerp(Data[index], r, blend);
                Data[index + 1] = MathHelpers.ByteLerp(Data[index + 1], g, blend);
                Data[index + 2] = MathHelpers.ByteLerp(Data[index + 2], b, blend);
                Data[index + 3] = 0xff;
            }
        }
    }
    
    public void Fill(int x0, int y0, int x1, int y1, int color)
    {
        FillBlend(x0, y0, x1, y1, color, 0);
    }
    
    public void FillBlend(int x0, int y0, int x1, int y1, int color, double blend)
    {
        if (_camera != null)
        {
            x0 += _camera.XOffset;
            x1 += _camera.XOffset;
            y0 += _camera.YOffset;
            y1 += _camera.YOffset;
        }
        
        FillBlendAbsolute(x0, y0, x1, y1, color, blend);
    }
    
    public void SetPixel(int x, int y, int color)
    {
        SetPixelBlend(x, y, color, 0);
    }

    public void SetPixelBlend(int x, int y, int color, double blend)
    {
        FillBlend(x, y, x, y, color, blend);
    }
    
    public bool IsOutOfBounds(int index)
    {
        return index < 0 || index >= Data.Length;
    }
    
    private static int _GetBytespaceIndex(int width, int x, int y)
    {
        return ((y * width) + x) * 4;
    }
    
}