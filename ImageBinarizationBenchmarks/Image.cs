using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Common;

[SupportedOSPlatform("windows")]
public struct Image
{
    public byte[] Pixels { get; }

    public byte[] GrayPixels { get; set; } = Array.Empty<byte>();

    public int Width { get; }
    public int Height { get; }

    public int BitmapDataStride { get; }

    public Image(string path)
    {
        var image = new Bitmap(path);
        var bitmapData = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadWrite, image.PixelFormat);

        Width = image.Width;
        Height = image.Height;
        var byteCount = bitmapData.Stride * image.Height;
        Pixels = new byte[byteCount];

        Marshal.Copy(bitmapData.Scan0, Pixels, 0, byteCount);

        image.UnlockBits(bitmapData);

        BitmapDataStride = bitmapData.Stride;
        GrayPixels = new byte[byteCount / 3];

        for (var i = 0; i < byteCount; i += 3)
        {
            var gray = (byte)(0.299 * Pixels[i + 2] +
                              0.587 * Pixels[i + 1] +
                              0.114 * Pixels[i]);
            GrayPixels[i / 3] = gray;
        }
    }

    public void Save(string path, bool isGray = false)
    {
        var pixelFormat = isGray ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;
        var pixels = isGray ? GrayPixels : Pixels;

        var image = new Bitmap(Width, Height, pixelFormat);
        var bitmapData = image.LockBits(
            new Rectangle(0, 0, Width, Height),
            ImageLockMode.ReadWrite, image.PixelFormat);

        Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);

        image.UnlockBits(bitmapData);
        image.Save(path);
    }

    public void SaveBmp(string path)
    {
        var pixelFormat = PixelFormat.Format1bppIndexed;

        var bitmap = new Bitmap(Width, Height, pixelFormat);
        bitmap.Palette.Entries[0] = Color.Black;
        bitmap.Palette.Entries[1] = Color.White;
        var bmpData = bitmap.LockBits(
            new Rectangle(0, 0, Width, Height),
            ImageLockMode.ReadWrite, bitmap.PixelFormat);

        int stride = bmpData.Stride;
        IntPtr scan0 = bmpData.Scan0;

        unsafe
        {
            byte* ptr = (byte*)scan0;
        
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x += 8)
                {
                    byte byteValue = 0;
        
                    for (int i = 0; i < 8; i++)
                    {
                        int pixelIndex = y * Width + x + i;
                        if (pixelIndex < GrayPixels.Length && GrayPixels[pixelIndex] > 0)
                        {
                            byteValue |= (byte)(0x80 >> i);
                        }
                    }
        
                    ptr[y * stride + x / 8] = byteValue;
                }
            }
        }

        bitmap.UnlockBits(bmpData);
        bitmap.Save(path);
    }
}