namespace ImageProcessingCS;

internal static class Helper
{
    // Extension method to map a function to each element of an array
    public static byte[] Map(this byte[] pixels, Func<byte, byte> mapFunction)
    {
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = mapFunction(pixels[i]);
        }
        return pixels;
    }
}