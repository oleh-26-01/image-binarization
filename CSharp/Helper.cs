namespace CSharp;

public static class Helper
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

    public static byte[] GenerateArray(int length)
    {
        var array = new byte[length];
        var random = new Random();
        for (var i = 0; i < length; i++)
        {
            array[i] = (byte)random.Next(0, 256);
        }
        return array;
    }
}